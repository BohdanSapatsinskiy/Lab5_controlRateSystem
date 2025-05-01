using controlRateSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using controlRateSystem.Data.Models;
using controlRateSystem.Data.Repositories;

namespace controlRateSystem.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IRatesRepository repository;

        public CategoryController(IRatesRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index()
        {
            var categories = repository.Categories.ToList();
            return View(categories);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                repository.CreateCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }


        public IActionResult Edit(int id)
        {
            var category = repository.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                repository.SaveCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        
        public IActionResult Delete(int id)
        {
            var category = repository.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = repository.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                repository.DeleteCategory(category);
            }
            return RedirectToAction("Index");
        }
    }
}
