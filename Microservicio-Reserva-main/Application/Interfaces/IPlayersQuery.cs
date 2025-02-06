using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPlayersQuery
    {
        Task<Players> GetPlayerById(int id);
        Task<bool> PlayerExists(int id);
       
    }
}
