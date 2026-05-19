using AutoMapper;
using BLL.DTOs;
using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class StockMovementService
    {
        private readonly PmsCSp26Context db;
        private readonly Mapper mapper;

        public StockMovementService(PmsCSp26Context db)
        {
            this.db = db;
            mapper = MapperConfig.GetMapper();
        }

        public List<StockMovementDTO> Get(string? searchTerm = null)
        {
            var query = db.StockMovements
                .Include(x => x.Product)
                .Include(x => x.PurchaseOrder)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x =>
                    x.ProductName.Contains(searchTerm) ||
                    (x.PurchaseOrderNumber != null && x.PurchaseOrderNumber.Contains(searchTerm)) ||
                    x.MovementType.Contains(searchTerm));
            }

            return mapper.Map<List<StockMovementDTO>>(query.OrderByDescending(x => x.CreatedAt).ToList());
        }

        public void Record(StockMovement movement)
        {
            db.StockMovements.Add(movement);
        }
    }
}