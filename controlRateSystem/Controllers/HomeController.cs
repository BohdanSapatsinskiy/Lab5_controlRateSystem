using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using controlRateSystem.Models;
using controlRateSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

using controlRateSystem.Data.Models;
using controlRateSystem.Data.Repositories;


namespace controlRateSystem.Controllers
{
    public class HomeController : Controller
    {
        private IRatesRepository repository;

        public HomeController(IRatesRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index(int? page, int? selectedCategoryId)
        {
            // Якщо є параметри то беремо їх, як ні то дані з сесії
            int currentPage = page ?? HttpContext.Session.GetInt32("CurrentPage") ?? 1;
            int currentCategory = selectedCategoryId ?? HttpContext.Session.GetInt32("SelectedCategoryId") ?? 0;

            // Запис в сесію
            HttpContext.Session.SetInt32("CurrentPage", currentPage);
            HttpContext.Session.SetInt32("SelectedCategoryId", currentCategory);

            int pageSize = 3;

            var currencies = repository.Currencies
                .Where(c => currentCategory == 0 || c.CategoryId == currentCategory)
                .OrderBy(c => c.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalItems = repository.Currencies
                .Where(c => currentCategory == 0 || c.CategoryId == currentCategory)
                .Count();

            var viewModel = new RatesListViewModel
            {
                Currencies = currencies,
                PagingInfo = new PagingInfo
                {
                    TotalItems = totalItems,
                    ItemsPerPage = pageSize,
                    CurrentPage = currentPage
                },
                SelectedCategoryId = currentCategory
            };

            ViewData["SelectedCategoryId"] = currentCategory;

            return View(viewModel);
        }
        //// Details
        public IActionResult Details(int id)
        {
            var currency = repository.Currencies
                .Include(c => c.Category)
                .FirstOrDefault(c => c.Id == id);

            if (currency == null) return NotFound();
            return View(currency);
        }

        /////////Create
        public IActionResult Create()
        {
            ViewBag.Categories = repository.Categories.ToList();
            return View(new Currency());
        }

        [HttpPost]
        public IActionResult Create(Currency currency)
        {
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                if (currency.PriceInDollar <= 0)
                {
                    ModelState.AddModelError("PriceInDollar", "Price must be a positive value.");
                }

                if (ModelState.IsValid)
                {
                    repository.CreateCurrency(currency);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Console.WriteLine("Model state is not valid!");
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
            }

            ViewBag.Categories = repository.Categories.ToList();
            return View(currency);
        }

        //// Edit
        public IActionResult Edit(int id)
        {
            var currency = repository.Currencies.FirstOrDefault(c => c.Id == id);
            if (currency == null) return NotFound();

            ViewBag.Categories = repository.Categories.ToList();

            return View(currency);
        }

        [HttpPost]
        public IActionResult Edit(Currency currency)
        {
            ModelState.Remove("Category");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is NOT valid!");
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine($"Saving currency: {currency.Name}, {currency.Code}, {currency.PriceInDollar}");

                repository.SaveCurrency(currency); 
                return RedirectToAction("Index");
            }

            ViewBag.Categories = repository.Categories.ToList();
            return View(currency);
        }



        ///Delete
        public IActionResult Delete(int id)
        {
            var currency = repository.Currencies.FirstOrDefault(c => c.Id == id);
            if (currency == null) return NotFound();
            return View(currency); // Підтвердження
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var currency = repository.Currencies.FirstOrDefault(c => c.Id == id);
            if (currency != null)
            {
                repository.DeleteCurrency(currency);
            }
            return RedirectToAction("Index");
        }



    }
}


