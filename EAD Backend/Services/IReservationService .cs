using EAD_Backend.Dto;
using EAD_Backend.NewFolder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAD_Backend.Services
{
    public interface IReservationService
    {
        Task<List<ReservationsWithSchedule>> GetAllAsync();
        Task<ReservationsWithSchedule> GetByIdAsync(string id);
        Task<List<Reservation>> GetByUserIdAsync(string userId);
        Task<CreateReservationDto> CreateAsync(CreateReservationDto reservation);
        Task<Reservation> UpdateAsync(string id, Reservation updatedReservation);
        Task<bool> DeleteById(string id);
    }
}
