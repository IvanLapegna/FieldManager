using Application.Interfaces;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class PlayersQuery : IPlayersQuery
    {
        private readonly ApiDbContext _context;

        public PlayersQuery(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<Players> GetPlayerById(int id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(player => player.UserID == id);

            return player;
        }

        public Task<bool> PlayerExists(int id)
        {
            return _context.Players.AnyAsync(player => player.UserID == id);
        }

       
    }
}
