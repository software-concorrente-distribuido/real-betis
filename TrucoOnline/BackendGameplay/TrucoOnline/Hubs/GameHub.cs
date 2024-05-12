using Microsoft.AspNetCore.SignalR;
using TrucoOnline.Models;

namespace TrucoOnline.Hubs {
    public class GameHub : Hub {

        public async void SubscribeToGame(Guid gameId) {
            var game = GameManager.games.Find(g => g.Id == gameId);
            await Groups.AddToGroupAsync(Context.ConnectionId, "game_" + gameId);
            await Clients.Caller.SendAsync("GameSubscribed", game);
        }

        public async void UnsubscribeFromGame(string gameId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "game_" + gameId);
        }

        public async void PlayCard(Guid gameId, Guid playerId, byte cardIndex) {
            var game = GameManager.games.Find(g => g.Id == gameId);
            if (game is null) {
                return;
            }
            if (game.CurrentPlayerIndex != game.Players.FindIndex(p => p.Id == playerId)) {
                Console.WriteLine("NOT THIS PLAYER TURN!");
                return;
            }
            game.PlayCard(playerId, cardIndex);
            await Clients.Group("game_" + gameId).SendAsync("CardPlayed", new { playerId, playedCard = game.LastPlayedCard, currentPlayer = game.CurrentPlayerIndex });

            if (game.IsLastRoundFinished()) {
                if (game.Rounds.Count < 3) {
                    StartRound(gameId);
                }
                else {
                    FinishGame(gameId);
                }
            }
        }

        public async void StartRound(Guid gameId) {
            var game = GameManager.games.Find(g => g.Id == gameId);
            var winner = game.LastRound.GetRoundWinner();
            game.StartRound();
            await Clients.Group("game_" + gameId).SendAsync("RoundStarted", new { newRound = game.LastRound, lastRoundWinner = winner, team1Points = game.Team1Points, team2Points = game.Team2Points });
        }

        public async void FinishGame(Guid gameId) {
            var game = GameManager.games.Find(g => g.Id == gameId);
            await Clients.Group("game_" + gameId).SendAsync("GameFinished", game);
        }
    }
}
