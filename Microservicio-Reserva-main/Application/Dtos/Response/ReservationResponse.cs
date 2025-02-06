using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response
{
    public class ReservationResponse
    {
        public Guid ReservationID { get; set; }

        public int OwnerUserID { get; set; }

        public FieldResponse Field { get; set; }

        public ReservationStatusResponse Status {  get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string AddPlayerLink { get; set; }
         
        public ICollection<PlayersResponse> Players {  get; set; }
    }
}
