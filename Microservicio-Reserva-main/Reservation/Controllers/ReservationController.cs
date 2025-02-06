using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationCreateRequest request)
        {
            try
            {

                var result = await reservationService.GenerateReservation(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (TimeSlotUnavailableException ex) 
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (OutOfTimeRangeException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (NoDayAvailabilityException  ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
           
   
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById([FromRoute] Guid id)
        {
            try {
                var result = await reservationService.GetReservationById(id);
                return  new JsonResult(result) { StatusCode = 200};
            }
            catch (Exception ex)
            {
                return NotFound(new { StatusCode = 404 });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationUpdateRequest reservationRequest, [FromRoute] Guid id )
        {
            try
            {
                var result = await reservationService.UpdateReservation(reservationRequest, id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [Authorize]
        [HttpDelete("{id}/Player/{pid}")]
        public async Task<IActionResult> DeletePlayerReservation([FromRoute] Guid id, [FromRoute] int pid)
        {
            try
            {
                await reservationService.DeletePlayerReservation(id, pid);
                return new JsonResult(new ApiResponse { Message = "El jugador fue eliminado de la reserva." }) { StatusCode = 200 };
            }
            catch (CannotDeleteOwnerException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (PlayerNotInReservationException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidReservationException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidPlayerException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation([FromRoute] Guid id)
        {
            try
            {
                await reservationService.CancelReservation(id);
                return new JsonResult(new ApiResponse { Message = "La reservacion fue cancelada." }) { StatusCode = 200 };
            }
            catch (InvalidReservationException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            

        }

        [Authorize]
        [HttpPost("add-player")]
        public async Task<IActionResult> AddPlayer([FromBody] PlayersRequest request)
        {
            try
            {
                await reservationService.AddPlayer(request);
                return new JsonResult(new ApiResponse { Message = "El jugador fue agregado exitosamente." }) { StatusCode = 200 };
                
            }
            catch (PlayerAlreadyExistsException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (CannotAddReservationOwnerException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (FullPlayersListException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidReservationException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidPlayerException ex)
            {
                return new JsonResult(new ApiResponse { Message = ex.Message }) { StatusCode = 400 };
            }

        }

        [Authorize]
        [HttpGet]       
        public async Task<IActionResult> GetAllReservations([FromQuery] int? pageSize, [FromQuery] int? pageNumber, int? user, Guid? field, string? date)
        {
            try
            {
                var reservations = await reservationService.GetAll(pageSize, pageNumber, user, field , date);
                return new JsonResult(reservations) { StatusCode = 200 };
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("Reservation-player")]
        public async Task<IActionResult> GetAllReservationsPlayer([FromQuery] int? pageSize, [FromQuery] int? pageNumber, int player)
        {
            try
            {
                var reservations = await reservationService.GetReservationsPlayer(pageSize, pageNumber, player);
                return new JsonResult(reservations) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

