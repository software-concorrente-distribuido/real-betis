namespace TrucoOnline.Models {
    public class Game {
        public Guid Id { get; set; }
        public Deck Deck { get; set; } = new Deck();
        public List<Player> Players { get; set; } = new List<Player>(4);
        public List<Round> Rounds { get; set; } = new List<Round>(3);
        public Round LastRound { get; set; }
        public Card LastPlayedCard { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int Team1Points { get; set; } = 0;
        public int Team2Points { get; set; } = 0;

        public Game() {
            Id = Guid.NewGuid();
            LastRound = new Round();
            Rounds.Add(LastRound);
        }

        public void Start() {
            foreach (var player in Players) {
                for (var i = 0; i < 3; i++) {
                    player.Cards.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);
                }
            }
        }

        public void PlayCard(Guid playerId, byte cardIndex) {
            var player = Players.First(p => p.Id == playerId);
            var card = player.Cards[cardIndex];
            player.Cards.RemoveAt(cardIndex);
            LastPlayedCard = card;

            LastRound.Cards.Push(new PlayedCard(card, player));
            NextPlayerTurn();
        }

        public void StartRound() {
            LastRound = new Round();
            Rounds.Add(LastRound);
        }

        public void FinishRound() {
            var winner = LastRound.GetRoundWinner();
            if (Players.IndexOf(winner.Player) % 2 == 0) {
                Team1Points++;
            }
            else {
                Team2Points++;
            }
        }

        public bool IsLastRoundFinished() {
            if (LastRound.Cards.Count == 4) {
                LastRound.IsConcluded = true;
            }

            return LastRound.IsConcluded;
        }

        public void NextPlayerTurn() {
            if (IsLastRoundFinished()) {
                var winner = LastRound.GetRoundWinner();
                if (winner is null) {
                    // Cangou 
                }
                else {
                    CurrentPlayerIndex = Players.IndexOf(winner.Player);
                    FinishRound();
                }
            }
            else {
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % 4;
            }
        }
    }
}
