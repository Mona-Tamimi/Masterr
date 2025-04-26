using master.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace master.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ===================== Register =====================
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var client = _httpClientFactory.CreateClient();

            model.Name = $"{model.FirstName} {model.LastName}";
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/users", content);
            return response.IsSuccessStatusCode ? RedirectToAction("Login") : View(model);
        }

        // ===================== Login =====================
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/users");
            if (!response.IsSuccessStatusCode) return View(model);

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<RegisterModel>>(json);

            var matchedUser = users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (matchedUser == null)
            {
                ViewBag.LoginError = "Invalid email or password.";
                return View(model);
            }

            // ✅ Always delete old LoggedUser (careful: may not reset ID in MockAPI)
            await client.DeleteAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser/1");

            // ✅ Save matched user in LoggedUser
            var loggedJson = JsonConvert.SerializeObject(matchedUser);
            var content = new StringContent(loggedJson, Encoding.UTF8, "application/json");
            await client.PostAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser", content);
            HttpContext.Session.SetString("RealUserId", matchedUser.Id);

            return RedirectToAction("Profile");
        }

        // ===================== Logout =====================
        public async Task<IActionResult> Logout()
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser/1");
            return RedirectToAction("Login");
        }

        // ===================== Profile =====================
        public async Task<IActionResult> Profile()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Login");

            var json = await response.Content.ReadAsStringAsync();
            var loggedUsers = JsonConvert.DeserializeObject<List<RegisterModel>>(json);

            var user = loggedUsers?.FirstOrDefault();
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        // ===================== Update Profile =====================
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(RegisterModel model, IFormFile ProfileImage)
        {
            var client = _httpClientFactory.CreateClient();

            // ✅ Step 1: Get current logged user (dynamic id!)
            var loggedUserResponse = await client.GetAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser");
            if (!loggedUserResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            var loggedUserJson = await loggedUserResponse.Content.ReadAsStringAsync();
            var loggedUsers = JsonConvert.DeserializeObject<List<RegisterModel>>(loggedUserJson);

            var currentUser = loggedUsers?.FirstOrDefault();
            if (currentUser == null)
                return RedirectToAction("Login");

            // ✅ Step 2: Assign real User ID for users API
            var realUserId = HttpContext.Session.GetString("RealUserId");

            if (string.IsNullOrEmpty(realUserId))
            {
                return RedirectToAction("Login");
            }

            model.Id = realUserId; // for USERS API update

            // ✅ Step 3: Preserve Password if missing
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = currentUser.Password;
            }

            // ✅ Step 4: Handle Profile Image Upload
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profile");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfileImage.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                model.Img = "/images/profile/" + fileName;
            }
            else
            {
                model.Img = currentUser.Img;
            }

            // ✅ Step 5: Split Name
            if (!string.IsNullOrEmpty(model.Name))
            {
                var nameParts = model.Name.Split(' ');
                model.FirstName = nameParts[0];
                model.LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";
            }

            // ✅ Step 6: Serialize and Update both APIs correctly
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 🛑 BIG FIX: Update LoggedUser using `currentUser.Id`
            await client.PutAsync($"https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser/{currentUser.Id}", content);

            // ✅ Update real user using `realUserId`
            await client.PutAsync($"https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/users/{realUserId}", content);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

    }
}
