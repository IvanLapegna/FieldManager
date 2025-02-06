using Application.Dtos.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _playersService;
        private readonly IEncryptionService _encryptionService;

        public PlayersController(IPlayersService playersService, IEncryptionService encryptionService)
        {
            _playersService = playersService;
            _encryptionService = encryptionService;
        }

        //private readonly IHttpClientFactory _httpClientFactory;




       



        //private async Task<bool> CheckUserExists(Guid userId)
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    var response = await client.GetAsync($"https://your-users-microservice/api/users/{userId}");
        //    return response.IsSuccessStatusCode;
        //}

    }

}
