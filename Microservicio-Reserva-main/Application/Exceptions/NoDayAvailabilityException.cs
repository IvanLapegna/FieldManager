using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NoDayAvailabilityException : Exception
    {
        public NoDayAvailabilityException() : base("No hay disponibilidad para la cancha en el dia de la fecha.") { }
    }
}
