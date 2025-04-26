using master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace master.Controllers
{
    public class ServiceController : Controller
    {

       private readonly MyDbContext _context;

        public ServiceController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult servicePage()
        {
            return View();
        }

        //public IActionResult Quiz()
        //{
        //    return View();
        //}

        public IActionResult Quiz(string category)
        {
            // Store the selected category in ViewBag to access it in the view
            if (!string.IsNullOrEmpty(category))
            {
                ViewBag.SelectedCategory = category;
            }
            return View();
        }

        public IActionResult SelectCategory()
        {
            return View();
        }

        public async Task<IActionResult> duringQuiz(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("SelectCategory");
            }

            // Store the selected category in ViewBag
            ViewBag.SelectedCategory = category;

            try
            {
                // Get the category ID from the database based on the name
                var categoryDb = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == category.ToLower());

                if (categoryDb == null)
                {
                    // Try to get any category if the specific one doesn't exist
                    // This is for testing purposes - remove in production
                    categoryDb = await _context.Categories.FirstOrDefaultAsync();

                    if (categoryDb == null)
                    {
                        ViewBag.ErrorMessage = $"Category '{category}' not found and no other categories exist.";
                        ViewBag.Questions = new List<Question>();
                        return View();
                    }

                    ViewBag.ErrorMessage = $"Category '{category}' not found. Using '{categoryDb.Name}' category for testing.";
                }

                // Get questions with their options for the category
                var questions = await _context.Questions
                    .Where(q => q.CategoryId == categoryDb.CategoryId)
                    .Include(q => q.QuestionOptions)
                    .AsNoTracking() // This helps with performance and avoids some tracking issues
                    .ToListAsync();

                if (questions == null || !questions.Any())
                {
                    ViewBag.ErrorMessage = $"No questions found for category '{categoryDb.Name}'. Database has {await _context.Questions.CountAsync()} total questions.";
                    ViewBag.Questions = new List<Question>();
                    return View();
                }

                // Check if each question has options
                foreach (var question in questions)
                {
                    if (question.QuestionOptions == null || !question.QuestionOptions.Any())
                    {
                        ViewBag.DebugInfo = $"Question ID {question.QuestionId} has no options";
                    }
                }

                // Pass questions to the view
                ViewBag.Questions = questions;
                ViewBag.QuestionCount = questions.Count;
                ViewBag.CategoryInfo = $"Category ID: {categoryDb.CategoryId}, Name: {categoryDb.Name}";

                return View();
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                ViewBag.ErrorMessage = "An error occurred while loading questions: " + ex.Message;
                ViewBag.Questions = new List<Question>();
                return View();
            }
        }

        //public IActionResult duringQuiz()
        //{
        //    return View();
        //}

        public IActionResult payment()
        {
            return View();
        }

        public IActionResult resultQuiz()
        {
            return View();
        }

        public IActionResult consultant()
        {
            return View();
        }

        public IActionResult consultantProfile()
        {
            return View();
        }

        public IActionResult chatAI()
        {
            return View();
        }
    }
}
