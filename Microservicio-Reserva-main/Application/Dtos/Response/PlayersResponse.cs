using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response
{
    public class PlayersResponse
    {
        public Guid ReservationID { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string ImageUrl { get; set; }


    }
}
