using Application.Dtos.Request;
using Application.Dtos.Response;
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
    public class ReservationCommand : IReservationCommand
    {
        private readonly ApiDbContext _context;

        public ReservationCommand(ApiDbContext context)
        {
            _context = context;
        }

        

        public async Task InsertReservation(Reservation reservation)
        {
            _context.Add(reservation);
            await _context.SaveChangesAsync();
        }


        public async Task<Reservation> UpdateReservation(Reservation reservation)
        {
            _context.Update(reservation);

            await _context.SaveChangesAsync();

            return reservation;
        }

        
        public async Task DeleteReservation(Reservation reservationCancel)
        {
          
            if (reservationCancel.Players != null) 
            {
                _context.Players.RemoveRange(reservationCancel.Players);
            }

            reservationCancel.ReservationStatusID = 3;
            _context.Update(reservationCancel);

            await _context.SaveChangesAsync();

        }
    }
}
