namespace App.Entity.Models
{
    public class TransectionHistory
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string RefId { get; set; } = string.Empty;
        public string? PaymentId { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public int TemplateQty { get; set; } = 0;
        public DateTime CreateAt { get; set; }
    }
}
