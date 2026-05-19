using System;
using System.Collections.Generic;

namespace DAL.EF.Tables
{
    public partial class PurchaseOrder
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = null!;

        public string SupplierName { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? ReceivedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        public virtual ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();

        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}