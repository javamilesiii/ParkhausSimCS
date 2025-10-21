using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ParkhausAPI.Data;
using ParkhausAPI.Models;

namespace ParkhausAPI.Controllers
{
    public class TicketsController(ParkingContext _context) : ODataController
    {
        [EnableQuery]
        public IQueryable<Tickets> Get()
        {
            return _context.Tickets;
        }

        [EnableQuery]
        public async Task<IActionResult> Get(string key, CancellationToken token)
        {
            var ticket = await _context.Tickets.FindAsync(key, token);
            return ticket != null ? Ok(ticket): NotFound();
        }

        public async Task<IActionResult> Post([FromBody] Tickets ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(ticket.Id))
            {
                ticket.Id = Guid.NewGuid().ToString();
            }
            else
            {
                var existingTicket = await _context.Tickets.FindAsync(ticket.Id);
                if (existingTicket != null)
                {
                    return BadRequest($"Ticket with ID {ticket.Id} already exists");
                }
            }

            if (ticket.PurchaseTime == default)
            {
                ticket.PurchaseTime = DateTime.Now;
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Created(ticket);
        }

        public async Task<IActionResult> Put(string key, [FromBody] Tickets ticket)
        {
            if (key != ticket.Id) return BadRequest("ID mismatch");

            var existingTicket = await _context.Tickets.FindAsync(key);
            if (existingTicket == null) return NotFound();

            existingTicket.Spot = ticket.Spot;
            existingTicket.PurchaseTime = ticket.PurchaseTime;
            existingTicket.ExitTime = ticket.ExitTime;
            existingTicket.IsPaid = ticket.IsPaid;

            await _context.SaveChangesAsync();
            return Updated(existingTicket);
        }

        public async Task<IActionResult> Patch(string key, [FromBody] Tickets ticket)
        {
            if (key != ticket.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingTicket = await _context.Tickets.FindAsync(key);
            if (existingTicket == null) return NotFound();

            existingTicket.Spot = ticket.Spot;
            existingTicket.PurchaseTime = ticket.PurchaseTime;
            existingTicket.ExitTime = ticket.ExitTime;
            existingTicket.IsPaid = ticket.IsPaid;

            await _context.SaveChangesAsync();
            return Updated(existingTicket);
        }

        public async Task<IActionResult> Delete(string key)
        {
            var ticket = await _context.Tickets.FindAsync(key);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}