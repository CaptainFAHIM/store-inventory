using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Controllers
{
    public class ProductController : Controller
    {
        ProductService productService;
        CategoryService categoryService;

        public ProductController(ProductService productService, CategoryService categoryService) { 
            this.productService = productService;
            this.categoryService = categoryService;
        }
        private void LoadCategories(int? selectedCategoryId = null)
        {
            ViewBag.Categories = new SelectList(categoryService.Get(), "Id", "Name", selectedCategoryId);
        }

        public IActionResult Index(string? searchTerm)
        {
            var data = productService.Get(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(data);
        }
        [HttpGet]
        public IActionResult Create() {
            LoadCategories();
            return View(new ProductDTO());
        }
        [HttpPost]
        public IActionResult Create(ProductDTO p) {
            if (ModelState.IsValid) { 
               var res = productService.Create(p);
                if (res == true) {
                    return RedirectToAction("Index");
                }
            }
            LoadCategories(p.Cid);
            return View(p);
            
        }
        [HttpGet]
        public IActionResult Edit(int id) { 
            var data = productService.Get(id);
            if (data == null)
            {
                return NotFound();
            }

            LoadCategories(data.Cid);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(ProductDTO p)
        {
            if (ModelState.IsValid)
            {
                var res = productService.Update(p);
                if (res == true)
                {
                    return RedirectToAction("Index");
                }
            }
            LoadCategories(p.Cid);
            return View(p);
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            var data = productService.Get(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }
        [HttpPost]
        public IActionResult Delete(int id, string Decision) {
            if (string.Equals(Decision, "Yes", StringComparison.OrdinalIgnoreCase)) { 
                productService.Delete(id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
