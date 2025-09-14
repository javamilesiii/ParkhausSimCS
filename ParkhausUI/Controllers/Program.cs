using ParkhausUI.Controllers;
using ParkhausUI.Models;
using ParkhausUI.Views;

namespace ParkhausSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CarParc carParc = new CarParc(5, 50, 1.5f);

            carParc.Display.TotalSpaces = carParc.GetTotalSpaces();

            new Simulation().StartSimulation(carParc);
        }
    }
}