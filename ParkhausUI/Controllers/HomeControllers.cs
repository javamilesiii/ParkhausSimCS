using Microsoft.AspNetCore.Mvc;
using ParkhausUI.Models;
using System.Text;
using System.Text.Json;

namespace ParkhausUI.Controllers
{
    public class HomeController : Controller
    { 
        private static readonly HttpClient httpClient = new HttpClient(); // Wiederverwenden der HttpClient-Instanz suchen
        private static readonly string apiUrl = "http://ParkhausAPI/odata/Tickets"; // In der AppSettings.json konfigurieren
        private static CarParc carParc = new CarParc(5, 50, 1.5f);

        public IActionResult Index()
        {
            return View(carParc);
        }

        [HttpPost]
        public async Task<IActionResult> EnterGarage(int floorNumber, int spotNumber)
        {
            await GetTicketsFromDatabase();

            if (carParc.Floors[floorNumber - 1].spots[spotNumber - 1].occupied)
            {
                ViewBag.Message = "Parkinglot already occupied";
                return View("Index", carParc);
            }
            carParc.Floors[floorNumber - 1].spots[spotNumber - 1].occupied = true;
            var spot = $"{floorNumber}{(spotNumber < 10 ? "0" + spotNumber : spotNumber.ToString())}";
            var ticket = carParc.TicketMachine.GenerateTicket(spot);

            try
            {
                await SaveTicketToDatabase(ticket);
                ViewBag.Message = "Car parked! Tickets ID: " + ticket.Id + " (Saved)";
            }
            catch
            {
                ViewBag.Message = "Car parked! Tickets ID: " + ticket.Id + " (Local only)";
            }

            return View("Index", carParc);
        }

        [HttpPost]
        public async Task<IActionResult> PayTicket(string ticketId)
        {
            var ticket = carParc.TicketMachine.GetTicketById(ticketId);
            if (ticket == null)
            {
                ViewBag.Message = "Tickets not found!";
                return View("Index", carParc);
            }
            var price = carParc.Floors[0].cashDesk.CalculateParkingPrice(ticket, DateTime.Now);

            if (!carParc.Floors[0].cashDesk.PayTicket(ticketId))
            {
                ViewBag.Message = "Tickets Not Found";
            }

            try
            {
                await UpdateTicketInDatabase(ticket, "pay");
                ViewBag.Message = "Payment successful! Price: " + price + " CHF (Updated)";
            }
            catch
            {
                ViewBag.Message = "Payment successful! Price: " + price + " CHF (Local only)";
            }

            return View("Index", carParc);
        }

        [HttpPost]
        public async Task<IActionResult> ExitGarage(int floorNumber, int spotNumber, string ticketId)
        {
            var spot = $"{floorNumber}{(spotNumber < 10 ? "0" + spotNumber : spotNumber.ToString())}";
            var ticket = carParc.TicketMachine.GetTicketById(ticketId);
            if (ticket == null)
            {
                ViewBag.Message = "Tickets not found!";
                return View("Index", carParc);
            }

            if (ticket.Spot != spot)
            {
                ViewBag.Message = "Wrong Parkinglot";
                return View("Index", carParc);
            }
            carParc.Floors[floorNumber - 1].spots[spotNumber - 1].occupied = false;

            carParc.TicketMachine.RemoveTicket(ticket);

            try
            {
                await UpdateTicketInDatabase(ticket, "exit");
                ViewBag.Message = "Thanks for using our garage!";
            }
            catch
            {
                ViewBag.Message = "Thanks for using our garage! (Local only)";
            }
            
            return View("Index", carParc);
        }

        private async Task SaveTicketToDatabase(Tickets tickets)
        {
            if (tickets.ExitTime == default(DateTime)) tickets.ExitTime = null;
            try
            {
                Console.WriteLine("Starting to save tickets...");
                Console.WriteLine($"API URL: {apiUrl}");

                var json = JsonSerializer.Serialize(tickets);
                Console.WriteLine($"JSON: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine("Making HTTP request...");
                var response = await httpClient.PostAsync(apiUrl, content);

                Console.WriteLine($"Response Status: {response.StatusCode}");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Body: {responseBody}");

                response.EnsureSuccessStatusCode();
                Console.WriteLine("Tickets saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private async Task UpdateTicketInDatabase(Tickets tickets, String type)
        {
            try
            {
                if (type == "pay") tickets.IsPaid = true;
                else if (type == "exit") tickets.ExitTime = DateTime.UtcNow;
                Console.WriteLine("Starting to update tickets...");
                Console.WriteLine($"API URL: {apiUrl}");

                var json = JsonSerializer.Serialize(tickets);
                Console.WriteLine($"JSON: {json}");
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine("Making HTTP request...");
                var response = await httpClient.PutAsync($"{apiUrl}('{tickets.Id}')", content);
                Console.WriteLine($"Response Status: {response.StatusCode}");

                response.EnsureSuccessStatusCode();
                Console.WriteLine("Tickets updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private async Task<Tickets[]> GetTicketsFromDatabase()
        {
            try
            {
                var response = await httpClient.GetAsync(apiUrl);
                Console.WriteLine($"GetTicketsFromDatabase Response Status: {response.StatusCode}");
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"GetTicketsFromDatabase response body: {body}");

                var tickets = JsonSerializer.Deserialize<Tickets>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return tickets != null ? new[] { tickets } : Array.Empty<Tickets>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTicketsFromDatabase error: {ex.Message}");
                return Array.Empty<Tickets>();
            }
        }

        public async Task<Tickets[]> GetTicketsAsync()
        {
            return await GetTicketsFromDatabase();
        }
    }
}