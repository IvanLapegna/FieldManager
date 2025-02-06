using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class TimeSlotUnavailableException : Exception
    {
        public TimeSlotUnavailableException() : base("El horario elegido para la reserva no se encuentra disponible") { }
    }
}
