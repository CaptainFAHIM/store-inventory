using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService categoryService;

        public CategoryController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View(categoryService.Get());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryDTO());
        }

        [HttpPost]
        public IActionResult Create(CategoryDTO category)
        {
            if (ModelState.IsValid && categoryService.Create(category))
            {
                TempData["Success"] = "Category added successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = categoryService.Get(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(CategoryDTO category)
        {
            if (ModelState.IsValid && categoryService.Update(category))
            {
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = categoryService.Get(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(int id, string decision)
        {
            if (string.Equals(decision, "Yes", StringComparison.OrdinalIgnoreCase))
            {
                var deleted = categoryService.Delete(id);
                TempData[deleted ? "Success" : "Error"] = deleted
                    ? "Category deleted successfully."
                    : "Category could not be deleted because products still use it.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}