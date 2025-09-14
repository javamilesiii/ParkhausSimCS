using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ParkhausAPI.Data;
using ParkhausAPI.Models;

namespace ParkingSimulator.API.Controllers
{
    public class TicketsController : ODataController
    {
        private readonly ParkingContext _context;

        public TicketsController(ParkingContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<Ticket> Get()
        {
            return _context.Tickets;
        }

        [EnableQuery]
        public async Task<IActionResult> Get(string key)  // Changed from 'id' to 'key'
        {
            var ticket = await _context.Tickets.FindAsync(key);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        public async Task<IActionResult> Post([FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(ticket.Id))
            {
                ticket.Id = Guid.NewGuid().ToString();
            }

            if (ticket.PurchaseTime == default)
            {
                ticket.PurchaseTime = DateTime.Now;
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Created(ticket);  // OData convention
        }

        public async Task<IActionResult> Put(string key, [FromBody] Ticket ticket)  // Changed from 'id' to 'key'
        {
            if (key != ticket.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingTicket = await _context.Tickets.FindAsync(key);
            if (existingTicket == null)
            {
                return NotFound();
            }

            existingTicket.ExitTime = ticket.ExitTime;
            existingTicket.IsPaid = ticket.IsPaid;

            await _context.SaveChangesAsync();
            return Updated(existingTicket);  // OData convention
        }

        public async Task<IActionResult> Delete(string key)  // Changed from 'id' to 'key'
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