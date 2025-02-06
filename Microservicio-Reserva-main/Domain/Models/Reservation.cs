using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Reservation
    {
        public Guid ReservationID { get; set; }

        public Guid FieldID { get; set; }

        public int OwnerUserID { get; set; }

        public int ReservationStatusID { get; set; }

        public DateOnly Date {  get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int MaxJugadores { get; set; }

        public virtual ReservationStatus StatusNavigator { get; set; }

        public virtual ICollection<Players> Players { get; set; }

        // public virtual Field FieldNavigator { get; set; }
    }
}
