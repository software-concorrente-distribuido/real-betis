using Microsoft.AspNetCore.SignalR;
using TrucoOnline.Models;
using TrucoOnline.Models.Cards;

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

        public async void PlayCard(Guid lobbyId, Guid playerId, byte cardIndex, bool playedHidden) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            if (lobby is null) {
                return;
            }
            if (lobby.Games.Last().CurrentPlayerIndex != lobby.Players.FindIndex(p => p.Id == playerId)) {
                Console.WriteLine("NOT THIS PLAYER TURN!");
                return;
            }
            lobby.PlayCard(playerId, cardIndex, playedHidden);
            var playedCard = lobby.Games.Last().LastPlayedCard;
            if (lobby.Games.Last().LastRound.Cards.First().PlayedHidden) {
                playedCard = new Hidden();
            }
            await Clients.Group("lobby_" + lobbyId).SendAsync("CardPlayed", new { playerId, playedCard, currentPlayer = lobby.Games.Last().CurrentPlayerIndex });

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
            if (lobby is null) {
                Console.WriteLine("LOBBY NOT FOUND!");
                return;
            }
            lobby.StartGame();
            await Clients.Group("lobby_" + lobbyId).SendAsync("NextGameStarted", new { game = lobby.Games.Last(), players = lobby.Players });
        }

        public async void ConnectGuestPlayerToLobby(Guid lobbyId, string playerName) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            if (lobby is null) {
                Console.WriteLine("LOBBY NOT FOUND!");
                return;
            }
            if (lobby.Players.Count == 4) {
                Console.WriteLine("LOBBY FULL!");
                return;
            }

            if (playerName.Length > 20) {
                playerName = playerName.Substring(0, 20);
            }

            var player = new Player(playerName);

            if (lobby.Players.Count == 0) {
                player.IsLobbyAdmin = true;
            }

            lobby.Players.Add(player);
            await Clients.Group("lobby_" + lobbyId).SendAsync("PlayerConnected", new { playerId = player.Id, player.DisplayName, player.IsLobbyAdmin });
        }

        public async void DisconnectPlayerFromLobby(Guid lobbyId, Guid playerId) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            if (lobby is null) {
                Console.WriteLine("LOBBY NOT FOUND!");
                return;
            }

            var player = lobby.Players.Find(p => p.Id == playerId);
            if (player is null) {
                Console.WriteLine("PLAYER NOT FOUND!");
                return;
            }

            if (player.IsLobbyAdmin) {
                if (lobby.Players.Count > 1) {
                    lobby.Players.First(p => !p.IsLobbyAdmin).IsLobbyAdmin = true;
                }
            }

            lobby.Players.Remove(player);
            await Clients.Group("lobby_" + lobbyId).SendAsync("PlayerDisconnected", new { playerId });
        }

        public void StartLobby(Guid lobbyId, Guid playerId) {
            var lobby = GameManager.Lobbies.Find(g => g.Id == lobbyId);
            if (lobby is null) {
                Console.WriteLine("LOBBY NOT FOUND!");
                return;
            }
            var player = lobby.Players.Find(p => p.Id == playerId);
            if (player is null) {
                Console.WriteLine("PLAYER NOT FOUND!");
                return;
            }
            if (!player.IsLobbyAdmin) {
                Console.WriteLine("PLAYER NOT ADMIN!");
                return;
            }

            GoToNextGame(lobbyId);
        }
    }
}
