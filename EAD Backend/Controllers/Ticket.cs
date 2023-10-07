using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDBExample.Models;

namespace EAD_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet] // get all tickets endpoint
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();

            if (tickets == null)
            {
                return NotFound(new { success = false, data = tickets, msg = "No tickets found" });
            }

            return Ok(new { success = true, data = tickets, msg = "Success" });
        }

        [HttpGet("getAll/{nic}")] // get all tickets by userId endpoint
        public async Task<IActionResult> GetAllByUserId(string nic)
        {
            var tickets = await _ticketService.GetAllByUserIdAsync(nic);

            if (tickets == null)
            {
                return NotFound(new { success = false, data = tickets, msg = "No tickets found for the given userId" });
            }

            return Ok(new { success = true, data = tickets, msg = "Success" });
        }

        [HttpPost] // create ticket endpoint
        public async Task<IActionResult> Create([FromBody] Ticket ticket)
        {
            var createdTicket = await _ticketService.CreateAsync(ticket);

            if (createdTicket == null)
            {
                return BadRequest(new { success = false, data = createdTicket, msg = "Failed to create a ticket" });
            }

            return Ok(new { success = true, data = createdTicket, msg = "Success" });
        }

        [HttpPatch("update")] // update ticket endpoint
        public async Task<IActionResult> Update([FromBody] Ticket ticket)
        {
            var updatedTicket = await _ticketService.UpdateAsync(ticket);

            if (updatedTicket == null)
            {
                return BadRequest(new { success = false, data = updatedTicket, msg = "Failed to update the ticket" });
            }

            return Ok(new { success = true, data = updatedTicket, msg = "Success" });
        }
    }
}
