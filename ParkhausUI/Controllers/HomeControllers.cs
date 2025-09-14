using Microsoft.AspNetCore.Mvc;
using ParkhausUI.Models;
using System.Text;
using System.Text.Json;

namespace ParkhausUI.Controllers
{
    public class HomeController : Controller
    {
        private static CarParc carParc = new CarParc(5, 50, 1.5f);

        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string apiUrl = "http://localhost:60002/odata/Tickets";

        public IActionResult Index()
        {
            return View(carParc);
        }

        [HttpPost]
        public async Task<IActionResult> EnterGarage(int floorNumber, int spotNumber)
        {
            var ticket = carParc.TicketMachine.GenerateTicket();

            carParc.Floors[floorNumber - 1].spots[spotNumber - 1].occupied = true;

            try
            {
                await SaveTicketToDatabase(ticket);
                ViewBag.Message = "Car parked! Ticket ID: " + ticket.Id + " (Saved)";
            }
            catch
            {
                ViewBag.Message = "Car parked! Ticket ID: " + ticket.Id + " (Local only)";
            }

            return View("Index", carParc);
        }

        [HttpPost]
        public async Task<IActionResult> PayTicket(string ticketId)
        {
            var ticket = carParc.TicketMachine.GetTicketById(ticketId);
            var price = carParc.Floors[0].cashDesk.CalculateParkingPrice(ticket, DateTime.Now);

            carParc.Floors[0].cashDesk.PayTicket(ticketId);

            try
            {
                await UpdateTicketInDatabase(ticket);
                ViewBag.Message = "Payment successful! Price: " + price + " CHF (Updated)";
            }
            catch
            {
                ViewBag.Message = "Payment successful! Price: " + price + " CHF (Local only)";
            }

            return View("Index", carParc);
        }

        [HttpPost]
        public IActionResult ExitGarage(int floorNumber, int spotNumber, string ticketId)
        {
            carParc.Floors[floorNumber - 1].spots[spotNumber - 1].occupied = false;

            var ticket = carParc.TicketMachine.GetTicketById(ticketId);
            carParc.TicketMachine.RemoveTicket(ticket);

            ViewBag.Message = "Thanks for using our garage!";
            return View("Index", carParc);
        }

        private async Task SaveTicketToDatabase(Ticket ticket)
        {
            var json = JsonSerializer.Serialize(ticket);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();
        }

        private async Task UpdateTicketInDatabase(Ticket ticket)
        {
            ticket.IsPaid = true;
            ticket.ExitTime = DateTime.UtcNow;

            var json = JsonSerializer.Serialize(ticket);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{apiUrl}('{ticket.Id}')", content);
            response.EnsureSuccessStatusCode();
        }
    }
}