using EAD_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAD_Backend.Services
{
    public interface ITrainService
    {
        Task<List<Train>> GetAsync();
        Task<Train> CreateAsync(Train train);
        Task<Train> GetByIdAsync(string id);
        Task UpdateTrain(string id, Train train);
        Task UpdateStatus(string id, StatusEnum status);
        Task<Train> GetByName(string name);
        Task<bool> DeleteAsync(string id);
    }
}
