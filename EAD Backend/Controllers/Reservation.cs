using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using EAD_Backend.Models;
using EAD_Backend.NewFolder;
using EAD_Backend.Services;

namespace EAD_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("getAll")] // Get all reservations
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAllAsync();

            if (reservations == null)
            {
                return NotFound(new { success = false, data = reservations, msg = "Records not found" });
            }

            return Ok(new { success = true, data = reservations, msg = "Success" });
        }

        [HttpGet("getById/{id}")] // Get a single reservation by ID
        public async Task<IActionResult> GetById(string id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound(new { success = false, data = reservation, msg = "Record not found" });
            }

            return Ok(new { success = true, data = reservation, msg = "Success" });
        }

        [HttpGet("get/{nic}")] // Get reservations by user ID
        public async Task<IActionResult> GetByUserId(string nic)
        {
            var reservations = await _reservationService.GetByUserIdAsync(nic);

            if (reservations == null)
            {
                return NotFound(new { success = false, data = reservations, msg = "Records not found" });
            }

            return Ok(new { success = true, data = reservations, msg = "Success" });
        }

        [HttpPost("create")] // Create a reservation
        public async Task<IActionResult> Create([FromBody] CreateReservationDto reservation)
        {
            var createdReservation = await _reservationService.CreateAsync(reservation);

            if (createdReservation == null)
            {
                return BadRequest(new { success = false, data = createdReservation, msg = "Record not found" });
            }

            return Ok(new { success = true, data = createdReservation, msg = "Success" });
        }

        [HttpPatch("update/{id}")] // Update a reservation by ID
        public async Task<IActionResult> Update(string id, [FromBody] Reservation reservation)
        {
            var updatedReservation = await _reservationService.UpdateAsync(id, reservation);

            if (updatedReservation == null)
            {
                return BadRequest(new { success = false, data = updatedReservation, msg = "Record not found" });
            }

            return Ok(new { success = true, data = updatedReservation, msg = "Success" });
        }


        [HttpDelete("delete/{id}")] // Delete a train schedule
        public async Task<IActionResult> DeleteById(string id)
        {
            var deleted = await _reservationService.DeleteById(id);

            if (!deleted)
            {
                return NotFound(new { success = false, msg = "Reservation not found" });
            }

            return Ok(new { success = true, msg = "Reservation deleted successfully" });
        }

    }
}
