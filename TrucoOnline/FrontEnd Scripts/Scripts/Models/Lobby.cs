using System;
using System.Collections.Generic;

namespace TrucoOnline.Models {
    public class Lobby {
        public Guid Id { get; set; }
        public List<Game> Games { get; set; }
        public List<Player> Players { get; set; }
        public byte Team1Points { get; set; }
        public byte Team2Points { get; set; }

        public Lobby() {
            Id = Guid.NewGuid();
            Games = new List<Game>();
            Players = new List<Player>(4);
            Team1Points = 0;
            Team2Points = 0;
        }

        public void StartGame() {
            var game = new Game();
            game.Start(Players);
            Games.Add(game);
        }

        public void FinishGame() {
        }

        public void PlayCard(Guid playerId, byte cardIndex, bool playedHidden) {
        }

        public void NextPlayerTurn() {
        }

        public void FinishGameRound() {
        }

        public void StartGameRound(bool isCangado) {
        }
    }
}
