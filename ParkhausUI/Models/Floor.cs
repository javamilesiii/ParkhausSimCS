namespace ParkhausUI.Models
{
    public class Floor
    {
        public ParkingSpot[] spots { get; set; }
        public CashDesk cashDesk { get; set; }
        public CarParc carParc { get; set; }
        public Floor(int spacesPerFloor, TicketMachine ticketmachine, CarParc carParc)
        {
            this.carParc = carParc;
            this.spots = new ParkingSpot[spacesPerFloor];
            this.cashDesk = new CashDesk(ticketmachine, carParc);
            this.spots = Enumerable.Range(0, spacesPerFloor)
                .Select(_ => new ParkingSpot())
                .ToArray();
        }
        public int CalculateFreeSpaces()
        {
            return spots.Where(spot => spot != null && !spot.occupied).Count();
        }
    }
}
