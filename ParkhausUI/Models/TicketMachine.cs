using ParkhausUI.Controllers;

namespace ParkhausUI.Models
{
    public class TicketMachine
    {
        public List<Tickets> ActiveTickets = new List<Tickets>();

        /*public TicketMachine()
        {
            HomeController _controller = new HomeController();
            ActiveTickets = _controller.GetTicketsAsync().GetAwaiter().GetResult().ToList();
        }*/

        public Tickets GenerateTicket(string Spot)
        {
            Tickets tickets = new Tickets(Spot);
            ActiveTickets.Add(tickets);
            return tickets;
        }
        public void RemoveTicket(Tickets tickets) => ActiveTickets.Remove(tickets);
        public Tickets? GetTicketById(string id) => ActiveTickets.FirstOrDefault(ticket => ticket.Id == id.Trim());
        
    }
}
