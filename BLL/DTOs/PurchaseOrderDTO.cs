using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class PurchaseOrderDTO
    {
        public int Id { get; set; }

        public string? OrderNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; } = null!;

        [StringLength(500)]
        public string? Notes { get; set; }

        public string? Status { get; set; } = PurchaseOrderStatus.Pending;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Qty { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit cost must be greater than zero.")]
        public double UnitCost { get; set; }

        public double TotalCost => Qty * UnitCost;

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? ReceivedAt { get; set; }

        public DateTime? CancelledAt { get; set; }
    }
}