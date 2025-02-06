using Application.Interfaces;
using Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationStatusController : ControllerBase
    {
        private readonly IReservationStatusService _reservationStatusService;

        public ReservationStatusController(IReservationStatusService reservationStatusService)
        {
            _reservationStatusService = reservationStatusService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reservationStatusService.GetAllStatus();
            return new JsonResult(result) { StatusCode = 200 };
        }


    }
}
