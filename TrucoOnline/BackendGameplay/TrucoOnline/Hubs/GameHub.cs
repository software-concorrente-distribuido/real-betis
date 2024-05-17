using Microsoft.AspNetCore.SignalR;
using TrucoOnline.Models;

namespace TrucoOnline.Hubs {
    public class GameHub : Hub {

        public async void SubscribeToLobby(Guid lobbyId) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            if (lobby is null) {
                throw new Exception("Erro ao buscar lobby");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, "lobby_" + lobbyId);
            await Clients.Caller.SendAsync("LobbySubscribed", lobby);
        }

        public async void UnsubscribeFromLobby(string lobbyId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "lobby_" + lobbyId);
        }

        public async void PlayCard(Guid lobbyId, Guid playerId, byte cardIndex) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            if (lobby is null) {
                return;
            }
            if (lobby.Games.Last().CurrentPlayerIndex != lobby.Players.FindIndex(p => p.Id == playerId)) {
                Console.WriteLine("NOT THIS PLAYER TURN!");
                return;
            }
            lobby.PlayCard(playerId, cardIndex);
            await Clients.Group("lobby_" + lobbyId).SendAsync("CardPlayed", new { playerId, playedCard = lobby.Games.Last().LastPlayedCard, currentPlayer = lobby.Games.Last().CurrentPlayerIndex });

            if (lobby.Games.Last().IsLastRoundFinished()) {
                if (lobby.Games.Last().IsGameFinished) {
                    FinishGame(lobbyId);
                }
                else {
                    StartRound(lobbyId);
                }
            }
        }

        public async void StartRound(Guid lobbyId) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            if (lobby is null) {
                throw new Exception("Erro ao buscar lobby");
            }
            var winner = lobby.Games.Last().LastRound.GetRoundWinner();
            var isCangado = winner is null;
            lobby.StartGameRound(isCangado);
            await Clients.Group("lobby_" + lobbyId).SendAsync("RoundStarted", new { newRound = lobby.Games.Last().LastRound, lastRoundWinner = winner, team1Points = lobby.Games.Last().Team1Points, team2Points = lobby.Games.Last().Team2Points });
        }

        public async void FinishGame(Guid lobbyId) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            lobby.FinishGame();
            await Clients.Group("lobby_" + lobbyId).SendAsync("GameFinished", new { Team1Points = lobby.Team1Points, Team2Points = lobby.Team2Points });

            if (lobby.Team1Points != 12 && lobby.Team2Points != 12) {
                GoToNextGame(lobbyId);
            }
        }

        public async void GoToNextGame(Guid lobbyId) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            lobby.StartGame();
            await Clients.Group("lobby_" + lobbyId).SendAsync("NextGameStarted", new { game = lobby.Games.Last(), players = lobby.Players });
        }
    }
}
