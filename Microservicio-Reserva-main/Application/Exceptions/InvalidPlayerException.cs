using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class InvalidPlayerException : Exception
    {
        public InvalidPlayerException() : base("El jugador no existe.") { }
    }
}
