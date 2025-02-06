using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request
{
    public class PlayersRequest
    {
        public Guid ReservationID { get; set; }
        public int UserID { get; set; }
       
    }
}
