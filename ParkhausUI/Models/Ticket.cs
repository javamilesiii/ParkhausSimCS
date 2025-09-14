namespace ParkhausUI.Models
{
    public class Ticket
    {
        public string Id { get; set; }
        public string Spot { get; set; }
        public DateTime PurchaseTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public bool IsPaid { get; set; }

        public Ticket(string Spot)
        {
            this.Id = $"T{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
            this.Spot = Spot;
            this.IsPaid = false;
            this.PurchaseTime = DateTime.UtcNow;
        }
    }
}
