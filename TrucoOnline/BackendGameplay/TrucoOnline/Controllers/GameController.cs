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
        public ActionResult<Game> NewGame() {
            try {
                var game = _gameService.GetGame();
                GameManager.games.Add(game);
                return Ok(game);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public ActionResult<Game> GetGame(
            [FromQuery(Name = "gameId")][Required(ErrorMessage = "Query Parameter gameId is required")] Guid gameId
            ) {
            try {
                var game = GameManager.games.FirstOrDefault(g => g.Id == gameId);
                if (game is null) {
                    return NotFound();
                }
                return Ok(game);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
