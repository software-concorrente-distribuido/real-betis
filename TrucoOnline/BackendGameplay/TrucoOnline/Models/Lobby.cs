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
            var game = Games.Last();
            var winner = game.GetGameWinner();
            if (winner == 1) {
                Team1Points++;
            }
            else if (winner == 2) {
                Team2Points++;
            }

            if (Team1Points == 12 || Team2Points == 12) {
                // Lobby finished
            }
            else {
                StartGame();
            }
        }

        public void PlayCard(Guid playerId, byte cardIndex) {
            var game = Games.Last();
            var player = Players.First(p => p.Id == playerId);
            game.PlayCard(player, cardIndex);
            NextPlayerTurn();
        }

        public void NextPlayerTurn() {
            var game = Games.Last();
            if (game.IsLastRoundFinished()) {
                var winner = game.LastRound.GetRoundWinner();
                if (winner is null) {
                    // Cangou 
                }
                else {
                    game.CurrentPlayerIndex = Players.IndexOf(winner.Player);
                    FinishGameRound();
                }
            }
            else {
                game.CurrentPlayerIndex = (game.CurrentPlayerIndex + 1) % 4;
            }
        }

        public void FinishGameRound() {
            Games.Last().FinishRound(Players);
        }

        public void StartGameRound() {
            Games.Last().StartRound();
        }
    }
}
