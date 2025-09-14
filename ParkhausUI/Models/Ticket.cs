namespace ParkhausUI.Models
{
    public class Ticket
    {
        public string Id { get; set; }
        public DateTime PurchaseTime { get; set; }
        public DateTime ExitTime { get; set; }
        public bool IsPaid { get; set; }

        public Ticket()
        {
            this.Id = GetHashCode().ToString();
            this.IsPaid = false;
            this.PurchaseTime = DateTime.UtcNow;
        }
    }
}
