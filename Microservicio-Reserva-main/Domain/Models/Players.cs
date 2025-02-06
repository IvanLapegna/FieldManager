using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Players
    {
        public Guid ReservationID { get; set; }

        public int UserID { get; set; }

        public virtual Reservation Reservation { get; set; }

        //public virtual User Player { get; set; }
    }
}
