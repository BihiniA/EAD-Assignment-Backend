using EAD_Backend.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAD_Backend.Services
{
    public interface ITrainScheduleService
    {
        Task<List<TrainSchedule>> GetAsync();
        Task<TrainSchedule> CreateAsync(CreateTrainScheduleDto createTrainScheduleDto);
        Task<TrainSchedule> GetByIdAsync(string id);
        Task UpdateTrainSchedule(string id, TrainSchedule updatedTrainSchedule);
        Task<bool> DeleteAsync(string id);
        Task<TrainSchedule> UpdateTrainScheduleSeatCount(string id, UpdateTrainScheduleSeatCountDto updateTrainScheduleSeatCountDto);

    }
}
