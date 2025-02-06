using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class FullPlayersListException : Exception
    {
        public FullPlayersListException() : base("No pueden agregarse mas jugadores, la lista esta llena.") { }
    }
}
