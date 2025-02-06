using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class CannotAddReservationOwnerException : Exception
    {
        public CannotAddReservationOwnerException() : base("El propietario de la reserva no puede añadirse como un nuevo jugador.") { }
    }
}
