using ParkhausUI.Models;

namespace ParkhausUI.Views
{
    public class PrintHandler
    {
        public void PrintFloor(Floor floor)
        {
            floor.spots.Select((spot, i) =>
            {
                if (spot != null && spot.occupied)
                {
                    Console.WriteLine("---  ");
                }
                else
                {
                    Console.WriteLine($"{i + 1:000}  ");
                }
                if ((i + 1) % 10 == 0 && i != floor.spots.Length - 1)
                {
                    Console.WriteLine();
                }
                return 0;
            }).ToList();
            Console.WriteLine();
        }

        public void PrintTicket(CarParc carParc)
        {
            Ticket ticket = carParc.TicketMachine.GenerateTicket();
            string ticketNumber = ticket.Id;
            string ticketTime = ticket.PurchaseTime.ToString("dd/MM/yyyy HH:mm:ss");

            Console.WriteLine("########################" +
                "\n#  Ticket Information  #" +
                "\n#----------------------#" +
                "\n#      Nr. " + ticketNumber + "      #" +
                "\n# " + ticketTime + "  #" +
                "\n########################");
        }
    }
}
