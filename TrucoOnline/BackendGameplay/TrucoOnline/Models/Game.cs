namespace TrucoOnline.Models {
    public class Game {
        public Guid Id { get; set; }
        public Deck Deck { get; set; }
        public List<Round> Rounds { get; set; }
        public Round LastRound { get; set; }
        public Card? LastPlayedCard { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public bool IsGameFinished { get; set; }

        public Game() {
            Id = Guid.NewGuid();
            Deck = new Deck();
            LastRound = new Round();
            Rounds = new List<Round>(3) { LastRound };
            Team1Points = 0;
            Team2Points = 0;
        }

        public void Start(List<Player> players) {
            foreach (var player in players) {
                player.Cards.Clear();
                for (var i = 0; i < 3; i++) {
                    player.Cards.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);
                }
            }
        }

        public void PlayCard(Player player, byte cardIndex) {
            var card = player.Cards[cardIndex];
            player.Cards.RemoveAt(cardIndex);
            LastPlayedCard = card;

            LastRound.Cards.Push(new PlayedCard(card, player));
        }

        public void StartRound() {
            LastRound = new Round();
            Rounds.Add(LastRound);
        }

        public void FinishRound(List<Player> players) {
            var winner = LastRound.GetRoundWinner();
            if (players.IndexOf(winner.Player) % 2 == 0) {
                Team1Points++;
            }
            else {
                Team2Points++;
            }

            if (Team1Points == 2 || Team2Points == 2) {
                IsGameFinished = true;
            }
        }

        public bool IsLastRoundFinished() {
            if (LastRound.Cards.Count == 4) {
                LastRound.IsConcluded = true;
            }

            return LastRound.IsConcluded;
        }

        public byte? GetGameWinner() {
            if (Team1Points == 2) {
                return 1;
            }
            else if (Team2Points == 2) {
                return 2;
            }

            return null;
        }
    }
}
