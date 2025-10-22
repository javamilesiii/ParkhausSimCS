namespace ParkhausUI.Models
{
    public class Floor(int spacesPerFloor, TicketMachine ticketmachine, CarParc carParc)
    {
        public ParkingSpot[] spots { get; set; } = Enumerable.Range(0, spacesPerFloor)
                .Select(_ => new ParkingSpot())
                .ToArray();
        public CashDesk cashDesk { get; set; } = new CashDesk(ticketmachine, carParc);
        public CarParc carParc { get; set; } = carParc;

        public int CalculateFreeSpaces()
        {
            return spots.Where(spot => spot != null && !spot.occupied).Count();
        }
    }
}
