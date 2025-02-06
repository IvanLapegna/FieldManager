using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class CannotDeleteOwnerException : Exception
    {
        public CannotDeleteOwnerException() : base("No puede eliminarse al responsable de la reserva.") { }
    }
}
