namespace ParkhausUI.Models
{
    public class Display
    {
        public int FreeSpaces { get; set; }
        public int TotalSpaces { get; set; }
        public DateTime LastUpdate { get; set; }

        public Display() => LastUpdate = DateTime.Now;

        public void UpdateFreeSpaces(int freeSpaces)
        {
            FreeSpaces = freeSpaces;
            LastUpdate = DateTime.Now;
        }
    }
}