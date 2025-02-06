using Application.Dtos.Request;
using Application.Dtos.Response;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponse> GenerateReservation(ReservationCreateRequest request);
        Task<ReservationResponse> GetReservationById(Guid reservationId);


        Task ReservationExists(Guid reservationId);
        Task AddPlayer(PlayersRequest player);
        Task DeletePlayerReservation(Guid reservationID, int UserID);
        Task<ReservationResponse> UpdateReservation(ReservationUpdateRequest reservation, Guid id);
        Task CancelReservation(Guid reservationID);
        Task<IList<ReservationResponse>> GetAll(int? pageSize, int? pageNumber, int? user, Guid? field, string? date);
        Task<IList<ReservationResponse>> GetReservationsPlayer(int? pageSize, int? pageNumber, int player);
    }
}
