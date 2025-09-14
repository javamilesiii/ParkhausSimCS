namespace ParkhausUI.Models
{
    public class TicketMachine
    {
        public List<Ticket> ActiveTickets = new List<Ticket>();

        public Ticket GenerateTicket(string Spot)
        {
            Ticket ticket = new Ticket(Spot);
            ActiveTickets.Add(ticket);
            return ticket;
        }
        public void RemoveTicket(Ticket ticket) => ActiveTickets.Remove(ticket);
        public Ticket GetTicketById(string id) => ActiveTickets.Where(ticket => ticket.Id == id.Trim()).FirstOrDefault() ?? throw new TicketNotFoundException(id);
        
    }
}
