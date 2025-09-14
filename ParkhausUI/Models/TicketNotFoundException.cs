namespace ParkhausUI.Models
{
    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException() { }
        public TicketNotFoundException(string message) : base(message) { }
        public TicketNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
