using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class PlayerNotInReservationException : Exception
    {
        public PlayerNotInReservationException() : base("El jugador no forma parte de la reserva.") { }
    }
}
