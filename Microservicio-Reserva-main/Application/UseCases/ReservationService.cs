using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Application.UseCases
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationCommand _reservationCommand;
        private readonly IReservationQuery _reservationQuery;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly IPlayersService _playersService;
        private readonly IHttpService _httpService;
        private readonly IPlayersCommand _playersCommand;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReservationService(IReservationCommand reservationCommand, IReservationQuery reservationQuery, IEncryptionService encryptionService, IMapper mapper, IPlayersService playersService, IHttpService httpService, IPlayersCommand playersCommand, IHttpContextAccessor contextAccessor)
        {
            _reservationCommand = reservationCommand;
            _reservationQuery = reservationQuery;
            _encryptionService = encryptionService;
            _mapper = mapper;
            _playersService = playersService;
            _httpService = httpService;
            _playersCommand = playersCommand;
            _contextAccessor = contextAccessor;
        }

        public async Task<ReservationResponse> GenerateReservation(ReservationCreateRequest request)
        {
            var authorizationHeader = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            var token = authorizationHeader?.Replace("Bearer ", "").Trim();

            var field = await _httpService.GetAsync<FieldResponse>($"https://localhost:7267/api/v1/Field/{request.FieldID}", token);

            ValidateFieldAvailability(request, field);

            await IsReserved(request);

            var reservation = _mapper.Map<Reservation>(request);

            reservation.ReservationStatusID = 1;

            var player = await _httpService.GetAsync<PlayersResponse>($"https://localhost:7130/api/v1/User/{request.UserID}", token);

            await _reservationCommand.InsertReservation(reservation);

            player.ReservationID = reservation.ReservationID;

            var ownerPlayer = new Players()
            {
                UserID = player.Id,
                ReservationID = reservation.ReservationID
            };

            await _playersCommand.InsertPlayer(ownerPlayer);

            var reservationResponse = _mapper.Map<ReservationResponse>(await GetReservationById(reservation.ReservationID));

            reservationResponse.AddPlayerLink = GenerateInvitationLink(reservation.ReservationID.ToString());

            reservationResponse.Field = field;

            var players = new List<PlayersResponse>();

            players.Add(player);

            reservationResponse.Players = players;

            return reservationResponse;
        }


        private async Task IsReserved(ReservationCreateRequest request)
        {
            var exists = await _reservationQuery.AlreadyExists(request);

            if (exists)
            {
                throw new TimeSlotUnavailableException();
            }
        }

        private void ValidateFieldAvailability(ReservationCreateRequest request, FieldResponse field)
        {
            var date = new DateTime(request.Year, request.Month, request.Day);
            var dayOfWeek = date.DayOfWeek;
            var openHour = TimeSpan.FromHours(request.StartHour);
            var closeHour = TimeSpan.FromHours(request.EndHour);

            var availabilitiesForDay = field.Availabilities.Where(a => a.Day == dayOfWeek.ToString()).ToList();

            if (!availabilitiesForDay.Any())
            {
                throw new NoDayAvailabilityException();
            }

            foreach (var availability in availabilitiesForDay)
            {
                if (openHour >= availability.OpenHour && closeHour <= availability.CloseHour)
                {
                    return;
                }
            }

            throw new OutOfTimeRangeException();
        }

        public async Task<ReservationResponse> GetReservationById(Guid id)
        {
            await ReservationExists(id);

            var reservation = await _reservationQuery.GetReservationById(id);

            var reservationResponse = await GetAllReservationData(reservation);

            return reservationResponse;
        }

        public async Task AddPlayer(PlayersRequest request)
        {

            await ReservationExists(request.ReservationID);

            var reservation = await _reservationQuery.GetReservationById(request.ReservationID);

            IsActive(reservation);

            await ValidateUserExists(request.UserID);

            var players = reservation.Players;


            ValidatePlayerToAdd(reservation, request.UserID);

            await _playersService.CreatePlayer(request, request.ReservationID);
        }

        private string GenerateInvitationLink(string reservationId)
        {
            var context = _contextAccessor.HttpContext.Request;

            var encryptedId = _encryptionService.Encrypt(reservationId);

            var baseUrl = $"{context.Scheme}://{context.Host}{context.PathBase}";

            var inviteLink = $"{baseUrl}/reservation/player-invitation?encryptedId={encryptedId}";

            return inviteLink;


        }

        //public async Task DeletePlayerReservation(Guid reservationID, int UserID)
        //{
        //    await ReservationExists(reservationID);

        //    var reservationExistence = await _reservationQuery.GetReservationById(reservationID);

        //    IsActive(reservationExistence);

        //    var player = await _playersService.GetPlayerById(UserID);
        //    //player.Reservation = reservationExistence;

        //    ValidatePlayerToDetele(reservationExistence, player.UserID);            

        //    reservationExistence.Players.Remove(player);

        //    await _reservationCommand.UpdateReservation(reservationExistence);          
        //}
        //

        public async Task DeletePlayerReservation(Guid reservationID, int UserID)
        {
            await ReservationExists(reservationID);

            var reservationExistence = await _reservationQuery.GetReservationById(reservationID);

            IsActive(reservationExistence);

            ValidatePlayerToDelete(reservationExistence, UserID);

            await _playersCommand.DeletePlayer(UserID, reservationID);
        }


        public async Task<ReservationResponse> UpdateReservation(ReservationUpdateRequest reservationUpdate, Guid id)
        {
            var authorizationHeader = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            var token = authorizationHeader?.Replace("Bearer ", "").Trim();

            await ReservationExists(id);

            var reservationExistence = await _reservationQuery.GetReservationById(id);

            IsActive(reservationExistence);

            var field = await _httpService.GetAsync<FieldResponse>($"https://localhost:7267/api/v1/Field/{reservationUpdate.FieldID}", token);

            var reservationRequest = _mapper.Map<ReservationCreateRequest>(reservationUpdate);

            ValidateFieldAvailability(reservationRequest, field);

            await IsReserved(reservationRequest);

            _mapper.Map(reservationUpdate, reservationExistence);

            await _reservationCommand.UpdateReservation(reservationExistence);

            var reservationResponse = await GetAllReservationData(reservationExistence);

            return reservationResponse;

        }

        public async Task CancelReservation(Guid reservationID)
        {
            await ReservationExists(reservationID);

            var reservationCancel = await _reservationQuery.GetReservationById(reservationID);

            IsActive(reservationCancel);

            await _reservationCommand.DeleteReservation(reservationCancel);
        }



        public async Task<IList<ReservationResponse>> GetAll(int? pageSize, int? pageNumber, int? user, Guid? field, string? date)
        {
            DateOnly? dateToParse = null;

            if (!string.IsNullOrEmpty(date))
            {
                dateToParse = DateOnly.ParseExact(date, "yyyy-MM-dd");
            }

            var reservationList = await _reservationQuery.GetAll(pageSize, pageNumber, user, field, dateToParse);

            var reservationResponse = new List<ReservationResponse>();

            foreach (var reservation in reservationList)
            {
                var response = await GetAllReservationData(reservation);
                reservationResponse.Add(response);
            }



            return reservationResponse;

        }

        public async Task<IList<ReservationResponse>> GetReservationsPlayer(int? pageSize, int? pageNumber, int player)
        {
            var reservationList = await _reservationQuery.GetReservationsPlayer(pageSize, pageNumber, player);

            var reservationResponse = new List<ReservationResponse>();

            foreach (var reservation in reservationList)
            {
                var response = await GetAllReservationData(reservation);
                reservationResponse.Add(response);
            }

            return reservationResponse;

        }

        private async Task<ReservationResponse> GetAllReservationData(Reservation reservation)
        {
            var authorizationHeader = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            var token = authorizationHeader?.Replace("Bearer ", "").Trim();

            var players = new List<PlayersResponse>();

            foreach (var player in reservation.Players)
            {
                var playerReponse = await _httpService.GetAsync<PlayersResponse>($"https://localhost:7130/api/v1/User/{player.UserID}", token);
                playerReponse.ReservationID = reservation.ReservationID;
                players.Add(playerReponse);
            }

            var fieldResponse = await _httpService.GetAsync<FieldResponse>($"https://localhost:7267/api/v1/Field/{reservation.FieldID}", token);


            var reservationResponse = new ReservationResponse
            {
                ReservationID = reservation.ReservationID,
                Players = players,
                Field = fieldResponse,
                AddPlayerLink = GenerateInvitationLink(reservation.ReservationID.ToString()),
                Status = new ReservationStatusResponse
                {
                    Id = reservation.ReservationStatusID,
                    Status = reservation.StatusNavigator.Status
                },
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Date = reservation.Date,
                OwnerUserID = reservation.OwnerUserID
            };

            return reservationResponse;
        }

        //Validaciones

        public async Task ReservationExists(Guid id)
        {
            var exists = await _reservationQuery.ReservationExists(id);

            if (!exists)
            {
                throw new InvalidReservationException();
            }
        }

        public void IsActive(Reservation reservation)
        {
            if (reservation.ReservationStatusID == 3)
            {
                throw new InvalidReservationException();
            }
        }


        private void ValidatePlayerToDelete(Reservation reservation, int playerId)
        {
            if (reservation.OwnerUserID == playerId)
            {
                throw new CannotDeleteOwnerException();

            }

            var exists = reservation.Players.Any(r => r.UserID == playerId);

            if (!exists)
            {
                throw new PlayerNotInReservationException();
            }
        }

        private void ValidatePlayerToAdd(Reservation reservation, int playerId)
        {
            var exists = reservation.Players.Any(r => r.UserID == playerId);

            if (reservation.OwnerUserID == playerId)
            {
                throw new CannotAddReservationOwnerException();
            }
            if (exists)
            {
                throw new PlayerAlreadyExistsException();
            }
            if (reservation.Players.Count == reservation.MaxJugadores)
            {
                throw new FullPlayersListException();
            }
        }

        private async Task ValidateUserExists(int playerId)
        {
            var authorizationHeader = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            var token = authorizationHeader?.Replace("Bearer ", "").Trim();

            var user = await _httpService.GetAsync<PlayersResponse>($"https://localhost:7130/api/v1/User/{playerId}", token);

            if (user == null)
            {
                throw new InvalidPlayerException();
            }
        }

    }
}
