using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TrucoOnline.Models;
using TrucoOnline.Services;

namespace TrucoOnline.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase {
        private readonly GameService _gameService;

        public GameController(GameService gameService) {
            _gameService = gameService;
        }
        [HttpPost]
        public ActionResult<Lobby> StartNewLobby() {
            try {
                var lobby = _gameService.StartNewLobby();
                GameManager.Lobbies.Add(lobby);
                return Ok(lobby);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public ActionResult<Lobby> GetGame(
            [FromQuery(Name = "lobbyId")][Required(ErrorMessage = "Query Parameter lobbyId is required")] Guid lobbyId
            ) {
            try {
                var lobby = GameManager.Lobbies.FirstOrDefault(g => g.Id == lobbyId);
                if (lobby is null) {
                    return NotFound();
                }
                return Ok(lobby);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("lobbies")]
        public ActionResult<List<Game>> FindAllLobbies()
        {
            var lobbies = GameManager.Lobbies;
            return Ok(lobbies);
        }
    }
}
