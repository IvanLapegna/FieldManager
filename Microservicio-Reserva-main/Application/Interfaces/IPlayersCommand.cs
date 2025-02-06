using Application.Dtos.Request;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPlayersCommand
    {
        Task InsertPlayer(Players player);
        Task DeletePlayer(int userId, Guid reservationId);
    }
}
