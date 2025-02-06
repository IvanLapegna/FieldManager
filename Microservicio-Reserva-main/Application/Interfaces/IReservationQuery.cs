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
    public interface IReservationQuery 
    {
        Task<bool> AlreadyExists(ReservationCreateRequest reservation);
        Task<Reservation> GetReservationById(Guid id);

        Task<IList<Reservation>> FilterReservations(DateOnly date, Guid fieldId);

        Task<bool> ReservationExists(Guid id);
        Task<IList<Reservation>> GetAll(int? pageSize, int? pageNumber, int? user, Guid? field, DateOnly? date);
        Task<IList<Reservation>> GetReservationsPlayer(int? pageSize, int? pageNumber, int player);

    }
}
