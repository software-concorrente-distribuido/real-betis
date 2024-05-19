using Microsoft.AspNetCore.SignalR.Client;
using RTeste.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Realtime {
    public class Program {
        static void Main(string[] args) {
            string hubUrl = "http://localhost:5089/gameHub";
            string invokeMethodName = "SubscribeToLobby";
            string onMethodName = "CardPlayed";
            string gameId = "5759526a-3ae2-4d85-bd6c-c4b14edbcc6d";
            Lobby lobby = null;
            object gameLock = new object();

            var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            connection.StartAsync().Wait();
            connection.On<CardPlayedEventDTO>(onMethodName, param => {
                lock (gameLock) {
                    Console.WriteLine("****** CARD PLAYED ******");
                    Console.WriteLine("\n");

                    lobby.Games.Last().CurrentPlayerIndex = param.CurrentPlayer;
                    lobby.Games.Last().LastRound.Cards.Push(new PlayedCard() { Card = param.PlayedCard, Player = lobby.Players.Find(p => p.Id == param.PlayerId) });
                }
            });

            connection.On<RoundStartedEventDTO>("RoundStarted", param => {
                lock (gameLock) {
                    Console.WriteLine("****** ROUND STARTED ******");
                    Console.WriteLine("\n");

                    if (param.LastRoundWinner != null) {
                        Console.WriteLine("Last Round Winner: " + param.LastRoundWinner.Player.DisplayName + " with " + param.LastRoundWinner.Card.Value + " of " + param.LastRoundWinner.Card.Suit);
                    }

                    if (param.NewRound.IsCangado) {
                        Console.WriteLine("*************");
                        Console.WriteLine("Cangado!!!");
                        Console.WriteLine("*************");
                    }
                    Console.WriteLine("Current score:");
                    Console.WriteLine("Team 1: " + param.Team1Points);
                    Console.WriteLine("Team 2: " + param.Team2Points);
                    Console.WriteLine();
                    lobby.Games.Last().Rounds.Add(param.NewRound);
                    lobby.Games.Last().LastRound = param.NewRound;
                }
            });

            connection.On<GameFinishedEventDTO>("GameFinished", param => {
                Console.WriteLine("****** GAME FINISHED ******");
                Console.WriteLine("\n");
                Console.WriteLine("**************************");
                Console.WriteLine("Team 1 Points: " + param.Team1Points);
                Console.WriteLine("Team 2 Points: " + param.Team2Points);
                Console.WriteLine("**************************");
            });

            connection.On<Lobby>("LobbySubscribed", param => {
                lock (gameLock) {
                    Console.WriteLine("****** GAME SUBSCRIBED ******");
                    Console.WriteLine("\n");
                    lobby = param;
                }
            });

            connection.On<NextGameStartedEventDTO>("NextGameStarted", param => {
                lock (gameLock) {
                    Console.WriteLine("****** NEXT GAME STARTED ******");
                    Console.WriteLine("\n");
                    lobby.Games.Add(param.Game);
                    lobby.Players = param.Players;
                }
            });

            connection.On<PlayerConnectedEventDTO>("PlayerConnected", param => {
                lock (gameLock) {
                    Console.WriteLine("****** PLAYER CONNECTED ******");
                    Console.WriteLine("\n");
                    Player player = new Player() { Id = param.PlayerId, DisplayName = param.DisplayName, IsLobbyAdmin = param.IsLobbyAdmin };
                    lobby.Players.Add(player);
                }
            });

            connection.On<PlayerDisconnectedEventDTO>("PlayerDisconnected", param => {
                lock (gameLock) {
                    Console.WriteLine("****** PLAYER DISCONNECTED ******");
                    Console.WriteLine("\n");
                    lobby.Players.Remove(lobby.Players.Find(p => p.Id == param.PlayerId));
                }
            });

            connection.InvokeAsync(invokeMethodName, gameId).Wait();
            connection.ServerTimeout = TimeSpan.FromMinutes(10);

            while (true) {
                Task.Delay(1000).Wait();
                while (lobby.Players.Count != 4) {
                    Console.Write($"Name for Player {lobby.Players.Count + 1}: ");
                    string playerName = Console.ReadLine();
                    connection.InvokeAsync("ConnectGuestPlayerToLobby", gameId, playerName).Wait();
                    Console.WriteLine();
                    Task.Delay(1000).Wait();
                }
                if (lobby.Games.Count == 0) {
                    connection.InvokeAsync("StartLobby", gameId, lobby.Players.First(p => p.IsLobbyAdmin).Id).Wait();
                    Task.Delay(1000).Wait();
                }
                lock (gameLock) {
                    Console.WriteLine("Current Player: " + lobby.Players[lobby.Games.Last().CurrentPlayerIndex].DisplayName);
                    if (lobby.Games.Last().LastRound.Cards.Count > 0) {
                        Console.WriteLine("********************");
                        Console.WriteLine("Current Round Cards:");
                        foreach (var card in lobby.Games.Last().LastRound.Cards) {
                            if (card.Card.Suit == CardSuit.Hidden) {
                                Console.WriteLine("-- HIDDEN -- ");
                                continue;
                            }
                            Console.WriteLine(card.Card.Value + " of " + card.Card.Suit);
                        }
                        Console.WriteLine("********************\n");
                    }
                    Console.WriteLine("Your Cards:");
                    foreach (var card in lobby.Players[lobby.Games.Last().CurrentPlayerIndex].Cards) {
                        Console.WriteLine(card.Value + " of " + card.Suit);
                    }
                    Console.Write("Choose a card to play: ");
                    string cardValue = Console.ReadLine();
                    Console.WriteLine("");
                    Console.WriteLine("Play hidden (Y/N): ");
                    string playHiddenString = Console.ReadLine();
                    bool playedHidden = false;

                    if (playHiddenString.ToUpper() == "Y") {
                        playedHidden = true;
                    }

                    connection.InvokeAsync("PlayCard", gameId, lobby.Players[lobby.Games.Last().CurrentPlayerIndex].Id, Convert.ToInt32(cardValue), playedHidden).Wait();
                    lobby.Players[lobby.Games.Last().CurrentPlayerIndex].Cards.RemoveAt(Convert.ToInt32(cardValue));
                }
            }
        }
    }
}
