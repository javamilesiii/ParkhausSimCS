namespace ParkhausUI.Models
{
    public class CashDesk
    {
        public TicketMachine TicketMachine { get; set; }
        public CarParc CarParc { get; set; }
        public CashDesk(TicketMachine ticketMachine, CarParc carParc)
        {
            this.TicketMachine = ticketMachine;
            this.CarParc = carParc;
        }
        public float CalculateParkingPrice(Ticket ticket, DateTime dateTime)
        {
            return (float)((int)Math.Floor((dateTime - ticket.PurchaseTime).TotalMinutes) * CarParc.PricePerMinute);
        }
        public bool PayTicket(string ticketId) 
        {
            var ticket = TicketMachine.GetTicketById(ticketId);
            if (ticket == null)
            {
                return false;
            }
            else
            {
                 return ticket.IsPaid = true;
            }
        }
    }
}
