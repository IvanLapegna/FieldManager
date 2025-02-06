using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class PlayersService: IPlayersService
    {
        private readonly IPlayersCommand _playersCommnad;
        private readonly IPlayersQuery _playersQuery;
        private readonly IMapper _mapper;

        public PlayersService(IPlayersCommand playersCommnad, IPlayersQuery playersQuery, IMapper mapper)
        {
            _playersCommnad = playersCommnad;
            _playersQuery = playersQuery;
            _mapper = mapper;
        }

        public async Task<PlayersResponse> CreatePlayer(PlayersRequest request, Guid reservationId)
        {
            var player = _mapper.Map<Players>(request);
            player.ReservationID = reservationId;
            await _playersCommnad.InsertPlayer(player);

            return _mapper.Map<PlayersResponse>(player);



        }

        public async Task<Players> GetPlayerById(int id)
        {
            await PlayerExists(id);

            var player = await _playersQuery.GetPlayerById(id);

            return player;
        }

        public async Task PlayerExists(int playerId)
        {
            var exists = await _playersQuery.PlayerExists(playerId);

            if (!exists)
            {
                throw new InvalidPlayerException();
            }
        }

        

    }
}
