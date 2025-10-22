namespace ParkhausUI.Models
{
    public class CashDesk(TicketMachine ticketMachine, CarParc carParc)
    {
        public TicketMachine TicketMachine { get; set; } = ticketMachine;
        public CarParc CarParc { get; set; } = carParc;

        public float CalculateParkingPrice(Tickets tickets, DateTime dateTime)
        {
            return (float)((int)Math.Floor((dateTime - tickets.PurchaseTime).TotalMinutes) * CarParc.PricePerMinute);
        }
        public bool PayTicket(string ticketId) 
        {
            var ticket = TicketMachine.GetTicketById(ticketId);
            return ticket != null && (ticket.IsPaid = true);
        }
    }
}
