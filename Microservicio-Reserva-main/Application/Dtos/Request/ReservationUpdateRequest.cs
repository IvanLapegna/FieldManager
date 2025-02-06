using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request
{
    public class ReservationUpdateRequest 
    {
        public Guid FieldID { get; set; }
     
        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int StartHour { get; set; }

        public int EndHour { get; set; }

        public int MaxJugadores { get; set; }
    }
}
