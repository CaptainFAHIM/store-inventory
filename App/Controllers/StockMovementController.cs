using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class StockMovementController : Controller
    {
        private readonly StockMovementService stockMovementService;

        public StockMovementController(StockMovementService stockMovementService)
        {
            this.stockMovementService = stockMovementService;
        }

        public IActionResult Index(string? searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;
            return View(stockMovementService.Get(searchTerm));
        }
    }
}