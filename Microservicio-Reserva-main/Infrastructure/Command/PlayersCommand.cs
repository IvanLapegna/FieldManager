using Application.Dtos.Request;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Command
{
    public class PlayersCommand: IPlayersCommand
    {
        private readonly ApiDbContext _context;

        public PlayersCommand(ApiDbContext context)
        {
            _context = context;
        }

        public async Task InsertPlayer(Players player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
        }
        public async Task DeletePlayer(int userId, Guid reservationId)
        {
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.UserID == userId && p.ReservationID == reservationId);

            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new PlayerNotInReservationException();
            }
        }
    }
}
