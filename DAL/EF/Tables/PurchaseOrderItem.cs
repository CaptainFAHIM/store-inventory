namespace DAL.EF.Tables
{
    public partial class PurchaseOrderItem
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int Qty { get; set; }

        public double UnitCost { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

        public virtual Product? Product { get; set; }
    }
}