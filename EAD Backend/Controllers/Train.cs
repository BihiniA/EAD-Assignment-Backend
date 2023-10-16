using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDBExample.Models;
using EAD_Backend.Models;
using EAD_Backend.Services;

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
            var existingTrain = await _trainService.GetByName(train.TrainName);
            if(existingTrain != null)
            {
                throw new Exception("Train already exist");
            }

            train.ScheduleId = new List<string>();
            train.Status = StatusEnum.ACTIVE;

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

            existingTrain.Status = train.Status; // Update Status
            existingTrain.SeatCount = train.SeatCount; // Update SeatCount

            await _trainService.UpdateTrain(id, existingTrain);
            return Ok(new { success = true, data = existingTrain, msg = "Train updated successfully" });
        }

        [HttpPatch("status/update/{id}")] // Update status for a train (restricted to USER)
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] StatusEnum status)
        {
            var existingTrain = await _trainService.GetByIdAsync(id);

            if (existingTrain == null)
            {
                return NotFound(new { success = false, msg = "Train not found" });
            }

            existingTrain.Status = status;

            await _trainService.UpdateStatus(id, status);
            return Ok(new { success = true, data = existingTrain, msg = "Train status updated successfully" });
        }

        [HttpDelete("{id}")] //delete user
        public async Task<IActionResult> Delete(string id)
        {
            await _trainService.DeleteAsync(id);
            return Ok();
        }
    }
}
