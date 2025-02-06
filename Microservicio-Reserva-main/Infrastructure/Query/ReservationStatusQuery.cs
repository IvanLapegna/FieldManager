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

namespace Infrastructure.Query
{
    public class ReservationStatusQuery : IReservationStatusQuery
    {
        private readonly ApiDbContext _context;

        public ReservationStatusQuery(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ReservationStatus>> GetAll()
        {
            var status = await _context.ReservationsStatus.ToListAsync();
               

            return status;     
        }
    }
}
