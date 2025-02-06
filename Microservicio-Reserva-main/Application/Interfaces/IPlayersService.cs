using Application.Dtos.Request;
using Application.Dtos.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPlayersService
    {
        Task<PlayersResponse> CreatePlayer(PlayersRequest request, Guid reservationId);
        Task<Players> GetPlayerById(int playerId);
        Task PlayerExists(int playerId);

    }
}

