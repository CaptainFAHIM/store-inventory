using System;

namespace DAL.EF.Tables
{
    public partial class StockMovement
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int? PurchaseOrderId { get; set; }

        public string? PurchaseOrderNumber { get; set; }

        public string MovementType { get; set; } = null!;

        public int QuantityChange { get; set; }

        public int PreviousQty { get; set; }

        public int NewQty { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Product? Product { get; set; }

        public virtual PurchaseOrder? PurchaseOrder { get; set; }
    }
}