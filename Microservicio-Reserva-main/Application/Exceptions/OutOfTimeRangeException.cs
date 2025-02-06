using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class OutOfTimeRangeException : Exception
    {
        public OutOfTimeRangeException() : base("El horario seleccionado esta fuera del rango de disponibilidad de la cancha.") { }
    }
}
