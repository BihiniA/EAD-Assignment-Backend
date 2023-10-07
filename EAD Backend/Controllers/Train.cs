using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDBExample.Models;

namespace EAD_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainController : Controller
    {
        private readonly TrainService _trainService;

        public TrainController(TrainService trainService)
        {
            _trainService = trainService;
        }

        [HttpGet] // get all train endpoint
        public async Task<IActionResult> Get()
        {
            var trains = await _trainService.GetAsync();

            if (trains == null)
            {
                return NotFound(new { success = false, data = trains, msg = "record not found" });
            }

            return Ok(new { success = true, data = trains, msg = "success" });
        }

        [HttpPost] // create train endpoint
        public async Task<IActionResult> Post([FromBody] Train train)
        {
            if (train.ScheduleId == null)
            {
                train.ScheduleId = new List<string>(); // Initialize ScheduleId as an empty list if it's null
            }

            var createdTrain = await _trainService.CreateAsync(train);

            if (createdTrain == null)
            {
                return BadRequest(new { success = false, data = createdTrain, msg = "record not found" });
            }

            return Ok(new { success = true, data = createdTrain, msg = "success" });
        }

        [HttpGet("{id}")] // Get a single train by id
        public async Task<IActionResult> GetById(string id)
        {
            var train = await _trainService.GetByIdAsync(id);

            if (train == null)
            {
                return NotFound(new { success = false, data = train, msg = "Record not found" });
            }

            return Ok(new { success = true, data = train, msg = "Success" });
        }

        [HttpPatch("update/{id}")] // Update train
        public async Task<IActionResult> UpdateTrain(string id, [FromBody] Train train)
        {
            var existingTrain = await _trainService.GetByIdAsync(id);

            if (existingTrain == null)
            {
                return NotFound(new { success = false, msg = "Train not found" });
            }

            // Check if user is authorized to perform this operation (e.g., based on user role)
            // If not authorized, return a 403 Forbidden response
            // Example authorization logic:
            // if (!User.IsInRole("USER"))
            // {
            //     return Forbid();
            // }

            // Update the train properties that you want to allow users to modify
            existingTrain._id = train._id; // Replace Property1 with the actual property name
            existingTrain.Status = train.Status; // Replace Property2 with the actual property name

            await _trainService.UpdateTrain(id, existingTrain);
            return Ok(new { success = true, data = existingTrain, msg = "Train updated successfully" });
        }

        [HttpPatch("status/update/{id}")] // Update status for a train (restricted to USER)
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
        {
            var existingTrain = await _trainService.GetByIdAsync(id);

            if (existingTrain == null)
            {
                return NotFound(new { success = false, msg = "Train not found" });
            }

            // Check if user is authorized to perform this operation (e.g., based on user role)
            // If not authorized, return a 403 Forbidden response
            // Example authorization logic:
            // if (!User.IsInRole("USER"))
            // {
            //     return Forbid();
            // }

            existingTrain.Status = status;

            await _trainService.UpdateStatus(id, status);
            return Ok(new { success = true, data = existingTrain, msg = "Train status updated successfully" });
        }
    }
}
