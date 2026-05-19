using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly PurchaseOrderService purchaseOrderService;
        private readonly ProductService productService;

        public PurchaseOrderController(PurchaseOrderService purchaseOrderService, ProductService productService)
        {
            this.purchaseOrderService = purchaseOrderService;
            this.productService = productService;
        }

        public IActionResult Index(string? searchTerm, string? status)
        {
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedStatus = status;
            ViewBag.Statuses = PurchaseOrderStatus.All;
            return View(purchaseOrderService.Get(searchTerm, status));
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadProducts();
            return View(new PurchaseOrderDTO { Status = PurchaseOrderStatus.Pending });
        }

        [HttpPost]
        public IActionResult Create(PurchaseOrderDTO order)
        {
            if (ModelState.IsValid && purchaseOrderService.Create(order))
            {
                TempData["Success"] = "Purchase order created successfully.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                TempData["Error"] = "Purchase order could not be created.";
            }

            LoadProducts(order.ProductId);
            return View(order);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var data = purchaseOrderService.Get(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var approved = purchaseOrderService.Approve(id);
            TempData[approved ? "Success" : "Error"] = approved
                ? "Purchase order approved."
                : "Purchase order could not be approved.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Receive(int id)
        {
            var received = purchaseOrderService.Receive(id);
            TempData[received ? "Success" : "Error"] = received
                ? "Purchase order received and stock was updated."
                : "Purchase order could not be received.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var cancelled = purchaseOrderService.Cancel(id);
            TempData[cancelled ? "Success" : "Error"] = cancelled
                ? "Purchase order cancelled."
                : "Purchase order could not be cancelled.";
            return RedirectToAction(nameof(Index));
        }

        private void LoadProducts(int? selectedProductId = null)
        {
            ViewBag.Products = new SelectList(productService.Get(), "Id", "Name", selectedProductId);
        }
    }
}