using Application.AutoMapper;
using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class ReservationQuery : IReservationQuery
    {
        private readonly ApiDbContext _context;

        public ReservationQuery(ApiDbContext context)
        {
            _context = context;
        }
       

        public async Task<bool> AlreadyExists(ReservationCreateRequest reservation)
        {           
            var date = new DateOnly(reservation.Year, reservation.Month, reservation.Day);
            var startHour = new TimeOnly(reservation.StartHour, 0);
            var endHour = new TimeOnly(reservation.EndHour, 0);

            var reservationsFiltered = await FilterReservations(date, reservation.FieldID);

            return reservationsFiltered.Any(r => r.Date == date && (r.StartTime < endHour && r.EndTime > startHour) && !(r.EndTime == startHour));
        }

        public async Task<Reservation> GetReservationById(Guid id)
        {
            var reservation = await _context.Reservations.Where(r => r.ReservationID == id)
                .Include(r => r.StatusNavigator)
                .Include(r => r.Players)
                .FirstOrDefaultAsync();
          
            return reservation;

        }
        public async Task<IList<Reservation>> GetAll(int? pageSize, int? pageNumber, int? user, Guid? field, DateOnly? date)
        {
            var query = _context.Reservations.AsQueryable();

            if (user.HasValue)
            {
                query = query.Where(r => r.OwnerUserID == user);   
            }

            if (field.HasValue)
            {
                query = query.Where(r => r.FieldID == field);
            }

            if (pageNumber.HasValue)
            {
                query = query.Skip(pageNumber.Value);
            }

            if (pageSize.HasValue)
            {
                if (pageSize.Value > 0)
                {
                    query = query.Take(pageSize.Value);
                }
                else
                {
                    return new List<Reservation>();
                }


            }

            if (date.HasValue)
            {
                query = query.Where(r => r.Date == date);   
            }

            query = query.Include(r => r.StatusNavigator)
                         .Include(r => r.Players);
           
            return await query.ToListAsync();
        }

        public async Task<bool> ReservationExists(Guid id)
        {
            return await _context.Reservations.AnyAsync(r => r.ReservationID == id);
        }

        public async Task<IList<Reservation>> FilterReservations(DateOnly date, Guid fieldId)
        {
            var reservations = await _context.Reservations.Where(r => r.Date == date && r.StatusNavigator.Id == 1 && r.FieldID == fieldId).ToListAsync();

            return reservations;
        }

        public async Task<IList<Reservation>> GetReservationsPlayer(int? pageSize, int? pageNumber, int player)
        {
            var query = _context.Reservations
                .Include(r => r.Players)
                .Where(r => r.Players.Any(p => p.UserID == player))
                .AsQueryable();


            if (pageNumber.HasValue)
            {
                query = query.Skip(pageNumber.Value);
            }

            if (pageSize.HasValue)
            {
                if (pageSize.Value > 0)
                {
                    query = query.Take(pageSize.Value);
                }
                else
                {
                    return new List<Reservation>();
                }


            }

            query = query.Include(r => r.StatusNavigator);

            return await query.ToListAsync();
        }



    }
}
