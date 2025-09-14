using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkhausAPI.Models
{
    public class Ticket
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public DateTime PurchaseTime { get; set; }

        public DateTime? ExitTime { get; set; }

        [Required]
        public bool IsPaid { get; set; } = false;
    }
}