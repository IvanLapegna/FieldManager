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
    public interface IReservationCommand
    {
        Task InsertReservation(Reservation reservation);

        Task<Reservation> UpdateReservation(Reservation reservation);

        Task DeleteReservation(Reservation reservation);

    }
}
