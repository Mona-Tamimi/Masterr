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

            // Clear old logged user
            await client.DeleteAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser/1");

            // Save current user as logged in
            var loggedJson = JsonConvert.SerializeObject(matchedUser);
            var content = new StringContent(loggedJson, Encoding.UTF8, "application/json");
            await client.PostAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser", content);

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
            var response = await client.GetAsync("https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser/1");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Login");

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<RegisterModel>(json);

            return View(user);
        }

        // ===================== Update Profile =====================
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(RegisterModel model, IFormFile ProfileImage)
        {
            var client = _httpClientFactory.CreateClient();

            // ✅ Handle profile image upload
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfileImage.FileName)}";

                // Define path to /wwwroot/images/profile
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profile");

                // Create directory if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Full file path
                var filePath = Path.Combine(folderPath, fileName);

                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // Set relative path for Img field
                model.Img = $"/images/profile/{fileName}";
            }

            // ✅ Preserve existing password if missing
            var getUserResponse = await client.GetAsync($"https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/users/{model.Id}");
            if (getUserResponse.IsSuccessStatusCode)
            {
                var userJson = await getUserResponse.Content.ReadAsStringAsync();
                var existingUser = JsonConvert.DeserializeObject<RegisterModel>(userJson);
                model.Password = existingUser?.Password; // preserve password
            }

            // ✅ Update both APIs
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PutAsync($"https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/LoggedUser/{model.Id}", content);
            await client.PutAsync($"https://67f96467094de2fe6ea16bac.mockapi.io/careerCompass/users/{model.Id}", content);
            TempData["SuccessMessage"] = "Your profile has been updated successfully!";

            return RedirectToAction("Profile");
        }




    }
}
