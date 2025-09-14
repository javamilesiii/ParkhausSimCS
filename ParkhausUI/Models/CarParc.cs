namespace ParkhausUI.Models
{
    public class CarParc
    {
        public Barrier EntranceBarrier { get; set; }
        public Barrier ExitBarrier { get; set; }
        public TicketMachine TicketMachine { get; set; }
        public Floor[] Floors { get; set; }
        public Display Display { get; set; }
        public int SpacesPerFloor { get; set; }
        public float PricePerMinute { get; set; }

        public CarParc(int floorCount, int spacesPerFloor, float pricePerMinute)
        {
            this.SpacesPerFloor = spacesPerFloor;
            this.PricePerMinute = pricePerMinute;
            this.TicketMachine = new TicketMachine();
            this.EntranceBarrier = new Barrier("Entrance Barrier");
            this.ExitBarrier = new Barrier("Exit Barrier");
            this.Floors = new Floor[floorCount];
            this.Display = new Display();

            this.Floors = Enumerable.Range(0, floorCount)
                .Select(_ => new Floor(spacesPerFloor, TicketMachine, this))
                .ToArray();

            UpdateDisplay();
        }

        public int GetTotalSpaces()
        {
            return Floors.Length * SpacesPerFloor;
        }

        public int GetFreeSpaces()
        {
            return Floors.Sum(floor => floor.CalculateFreeSpaces());
        }

        public void UpdateDisplay() => Display.UpdateFreeSpaces(GetFreeSpaces());
    }
}