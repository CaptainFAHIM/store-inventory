namespace BLL.DTOs
{
    public static class PurchaseOrderStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Received = "Received";
        public const string Cancelled = "Cancelled";

        public static readonly string[] All =
        {
            Pending,
            Approved,
            Received,
            Cancelled
        };
    }
}