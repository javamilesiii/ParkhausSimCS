using System.ComponentModel.DataAnnotations;

namespace ParkhausAPI.Models
{
    public class Tickets
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(3)]
        public string Spot { get; set; }

        [Required]
        public DateTime PurchaseTime { get; set; }

        public DateTime? ExitTime { get; set; }

        [Required]
        public bool IsPaid { get; set; } = false;
    }
}