using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDBExample.Models;
using EAD_Backend.Dto;
using EAD_Backend.Services;

namespace EAD_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TrainScheduleController : Controller
    {

        private readonly TrainScheduleService _trainScheduleService;

        public TrainScheduleController(TrainScheduleService _trainScheduleService)
        {
            this._trainScheduleService = _trainScheduleService;
        }

        [HttpGet("getAll")] // Get all train schedules
        public async Task<IActionResult> GetAll()
        {
            var trainSchedules = await _trainScheduleService.GetAsync();

            if (trainSchedules == null)
            {
                return NotFound(new { success = false, data = trainSchedules, msg = "No train schedules found" });
            }

            return Ok(new { success = true, data = trainSchedules, msg = "Success" });
        }

        [HttpPost("create")] // Create a new train schedule
        public async Task<IActionResult> Create([FromBody] CreateTrainScheduleDto createTrainScheduleDto)
        {
            var createdTrainSchedule = await _trainScheduleService.CreateAsync(createTrainScheduleDto);

            if (createdTrainSchedule == null)
            {
                return BadRequest(new { success = false, data = createdTrainSchedule, msg = "Failed to create train schedule" });
            }

            return Ok(new { success = true, data = createdTrainSchedule, msg = "Train schedule created successfully" });
        }

        [HttpPatch("update/{id}")] // Update a train schedule
        public async Task<IActionResult> Update(string id, [FromBody] TrainSchedule trainSchedule)
        {
            var existingTrainSchedule = await _trainScheduleService.GetByIdAsync(id);

            if (existingTrainSchedule == null)
            {
                return NotFound(new { success = false, msg = "Train schedule not found" });
            }

            await _trainScheduleService.UpdateTrainSchedule(id, trainSchedule);
            return Ok(new { success = true, msg = "Train schedule updated successfully" });
        }

        [HttpDelete("delete/{id}")] // Delete a train schedule
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _trainScheduleService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { success = false, msg = "Train schedule not found" });
            }

            return Ok(new { success = true, msg = "Train schedule deleted successfully" });
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var train = await _trainScheduleService.GetByIdAsync(id);

            if (train == null)
            {
                return NotFound(new { success = false, data = train, msg = "Record not found" });
            }

            return Ok(new { success = true, data = train, msg = "Success" });
        }
    }
}

