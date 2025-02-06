using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class PlayerAlreadyExistsException : Exception
    {
        public PlayerAlreadyExistsException() : base("El jugador ya forma parte de la reserva.") { }
    }
}
