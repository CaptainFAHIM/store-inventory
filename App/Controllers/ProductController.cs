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

        public IActionResult Index(string? searchTerm, int? categoryId, bool lowStockOnly = false)
        {
            var allProducts = productService.Get();
            var data = productService.Get(searchTerm, categoryId, lowStockOnly);
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.LowStockOnly = lowStockOnly;
            ViewBag.TotalProducts = allProducts.Count;
            ViewBag.TotalCategories = categoryService.Get().Count;
            ViewBag.LowStockProducts = allProducts.Count(x => x.Qty <= 5);
            ViewBag.OutOfStockProducts = allProducts.Count(x => x.Qty <= 0);
            ViewBag.InventoryValue = allProducts.Sum(x => x.Price * x.Qty);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Categories = new SelectList(categoryService.Get(), "Id", "Name", categoryId);
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
                    TempData["Success"] = "Product added successfully.";
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
                    TempData["Success"] = "Product updated successfully.";
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
                var deleted = productService.Delete(id);
                TempData[deleted ? "Success" : "Error"] = deleted
                    ? "Product deleted successfully."
                    : "Product could not be deleted.";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var data = productService.Get(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult IncreaseQty(int id)
        {
            var product = productService.Get(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            product.Qty += 1;
            if (productService.Update(product))
            {
                TempData["Success"] = $"{product.Name} quantity increased.";
            }
            else
            {
                TempData["Error"] = "Could not update the quantity.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DecreaseQty(int id)
        {
            var product = productService.Get(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            if (product.Qty <= 0)
            {
                TempData["Error"] = $"{product.Name} is already at zero stock.";
                return RedirectToAction("Index");
            }

            product.Qty -= 1;
            if (productService.Update(product))
            {
                TempData["Success"] = $"{product.Name} quantity decreased.";
            }
            else
            {
                TempData["Error"] = "Could not update the quantity.";
            }

            return RedirectToAction("Index");
        }
    }
}
