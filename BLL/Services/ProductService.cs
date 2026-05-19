using AutoMapper;
using BLL.DTOs;
using DAL.EF;
using DAL.EF.Tables;
using DAL.Repos;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ProductService
    {
        ProductRepo repo;
        PmsCSp26Context db;
        StockMovementService stockMovementService;
        Mapper mapper;
        public ProductService(ProductRepo repo, PmsCSp26Context db, StockMovementService stockMovementService)
        {
            this.repo = repo;
            this.db = db;
            this.stockMovementService = stockMovementService;
            mapper = MapperConfig.GetMapper();
        }
        public List<ProductDTO> Get(string? searchTerm = null, int? categoryId = null, bool lowStockOnly = false)
        {
            var data = repo.Get(searchTerm, categoryId, lowStockOnly);
            var res = mapper.Map<List<ProductDTO>>(data);
            return res;
        }
        public ProductDTO? Get(int id)
        {
            var data = repo.Get(id);
            var res = mapper.Map<ProductDTO>(data);
            return res;
        }
        public bool Create(ProductDTO c)
        {
            var data = mapper.Map<Product>(c);
            var res = repo.Create(data);
            return res;

        }
        public bool Update(ProductDTO c)
        {
            var data = mapper.Map<Product>(c);
            var res = repo.Update(data);
            return res;
        }
        public bool Delete(int id)
        {
            return repo.Delete(id);
        }

        public bool TryAdjustQuantity(int id, int delta, string movementType, string? notes = null)
        {
            using var transaction = db.Database.BeginTransaction();

            var product = db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return false;
            }

            var previousQty = product.Qty;
            var newQty = previousQty + delta;
            if (newQty < 0)
            {
                return false;
            }

            product.Qty = newQty;

            stockMovementService.Record(new StockMovement
            {
                ProductId = product.Id,
                ProductName = product.Name,
                MovementType = movementType,
                QuantityChange = delta,
                PreviousQty = previousQty,
                NewQty = newQty,
                Notes = notes,
                CreatedAt = DateTime.Now
            });

            var saved = db.SaveChanges() > 0;
            if (saved)
            {
                transaction.Commit();
            }

            return saved;
        }
    }
}
