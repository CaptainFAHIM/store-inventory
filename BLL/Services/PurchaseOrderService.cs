using AutoMapper;
using BLL.DTOs;
using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class PurchaseOrderService
    {
        private readonly PmsCSp26Context db;
        private readonly Mapper mapper;
        private readonly StockMovementService stockMovementService;

        public PurchaseOrderService(PmsCSp26Context db, StockMovementService stockMovementService)
        {
            this.db = db;
            this.stockMovementService = stockMovementService;
            mapper = MapperConfig.GetMapper();
        }

        public List<PurchaseOrderDTO> Get(string? searchTerm = null, string? status = null)
        {
            var query = db.PurchaseOrders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x =>
                    x.OrderNumber.Contains(searchTerm) ||
                    x.SupplierName.Contains(searchTerm) ||
                    x.Items.Any(i => i.ProductName.Contains(searchTerm)));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.Status == status);
            }

            return mapper.Map<List<PurchaseOrderDTO>>(query.OrderByDescending(x => x.CreatedAt).ToList());
        }

        public PurchaseOrderDTO? Get(int id)
        {
            var order = db.PurchaseOrders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == id);

            return mapper.Map<PurchaseOrderDTO?>(order);
        }

        public bool Create(PurchaseOrderDTO dto)
        {
            var product = db.Products.FirstOrDefault(x => x.Id == dto.ProductId);
            if (product == null || dto.Qty <= 0 || dto.UnitCost <= 0)
            {
                return false;
            }

            using var transaction = db.Database.BeginTransaction();

            var order = new PurchaseOrder
            {
                OrderNumber = GenerateOrderNumber(),
                SupplierName = dto.SupplierName.Trim(),
                Notes = dto.Notes?.Trim(),
                Status = PurchaseOrderStatus.Pending,
                CreatedAt = DateTime.Now
            };

            db.PurchaseOrders.Add(order);
            db.SaveChanges();

            db.PurchaseOrderItems.Add(new PurchaseOrderItem
            {
                PurchaseOrderId = order.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                Qty = dto.Qty,
                UnitCost = dto.UnitCost
            });

            var saved = db.SaveChanges() > 0;
            if (saved)
            {
                transaction.Commit();
            }

            return saved;
        }

        public bool Approve(int id)
        {
            var order = db.PurchaseOrders.FirstOrDefault(x => x.Id == id);
            if (order == null || order.Status != PurchaseOrderStatus.Pending)
            {
                return false;
            }

            order.Status = PurchaseOrderStatus.Approved;
            order.ApprovedAt = DateTime.Now;
            return db.SaveChanges() > 0;
        }

        public bool Receive(int id)
        {
            using var transaction = db.Database.BeginTransaction();

            var order = db.PurchaseOrders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == id);

            if (order == null || order.Status != PurchaseOrderStatus.Approved)
            {
                return false;
            }

            foreach (var item in order.Items)
            {
                var product = item.Product ?? db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product == null)
                {
                    return false;
                }

                var previousQty = product.Qty;
                var newQty = previousQty + item.Qty;
                product.Qty = newQty;

                stockMovementService.Record(new StockMovement
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PurchaseOrderId = order.Id,
                    PurchaseOrderNumber = order.OrderNumber,
                    MovementType = "Purchase Receipt",
                    QuantityChange = item.Qty,
                    PreviousQty = previousQty,
                    NewQty = newQty,
                    Notes = $"Received from purchase order {order.OrderNumber}",
                    CreatedAt = DateTime.Now
                });
            }

            order.Status = PurchaseOrderStatus.Received;
            order.ReceivedAt = DateTime.Now;

            var saved = db.SaveChanges() > 0;
            if (saved)
            {
                transaction.Commit();
            }

            return saved;
        }

        public bool Cancel(int id)
        {
            var order = db.PurchaseOrders.FirstOrDefault(x => x.Id == id);
            if (order == null || order.Status == PurchaseOrderStatus.Received || order.Status == PurchaseOrderStatus.Cancelled)
            {
                return false;
            }

            order.Status = PurchaseOrderStatus.Cancelled;
            order.CancelledAt = DateTime.Now;
            return db.SaveChanges() > 0;
        }

        private static string GenerateOrderNumber()
        {
            return $"PO-{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}