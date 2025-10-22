namespace ParkhausUI.Models
{
    public class Tickets(string Spot)
    {
        public string Id { get; set; } = $"T{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        public string Spot { get; set; } = Spot;
        public DateTime PurchaseTime { get; set; } = DateTime.UtcNow;
        public DateTime? ExitTime { get; set; }
        public bool IsPaid { get; set; } = false;
    }
}