using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ReservationStatus
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
