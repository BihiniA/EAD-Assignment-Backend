using System;

namespace EAD_Backend.Models
{
    public class ReservationSearchModel
    {
        public string TrainScheduleid { get; set; }
        public string nic { get; set; }
        public DateTime? ReservationDate { get; set; }
        public int? ReserveCount { get; set; }
        public StatusEnum? Status { get; set; }
    }
}
