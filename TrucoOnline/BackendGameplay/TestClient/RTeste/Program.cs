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
            string gameId = "3568c637-292d-4016-a4a9-cb119d7c5fea";
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

                    Console.WriteLine("Last Round Winner: " + param.LastRoundWinner.Player.DisplayName + " with " + param.LastRoundWinner.Card.Value + " of " + param.LastRoundWinner.Card.Suit);
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

            connection.InvokeAsync(invokeMethodName, gameId).Wait();

            while (true) {
                Task.Delay(1000).Wait();
                lock (gameLock) {
                    Console.WriteLine("Current Player: " + lobby.Players[lobby.Games.Last().CurrentPlayerIndex].DisplayName);
                    if (lobby.Games.Last().LastRound.Cards.Count > 0) {
                        Console.WriteLine("********************");
                        Console.WriteLine("Current Round Cards:");
                        foreach (var card in lobby.Games.Last().LastRound.Cards) {
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
                    connection.InvokeAsync("PlayCard", gameId, lobby.Players[lobby.Games.Last().CurrentPlayerIndex].Id, Convert.ToInt32(cardValue)).Wait();
                    lobby.Players[lobby.Games.Last().CurrentPlayerIndex].Cards.RemoveAt(Convert.ToInt32(cardValue));
                }
            }
        }
    }
}
