using System;
using System.Collections.Generic;

namespace TrucoOnline.Models {
    [Serializable]
    public class Game {
        public Guid Id { get; set; }
        public Deck Deck { get; set; }
        public List<Round> Rounds { get; set; }
        public Round LastRound { get; set; }
        public Card LastPlayedCard { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public bool IsGameFinished { get; set; }
        public bool IsTrucado { get; set; }
        public Guid? PlayerTrucadoId { get; set; }
        public byte GameValue { get; set; }

        public Game() {
            Id = Guid.NewGuid();
            Deck = new Deck();
            LastRound = new Round();
            Rounds = new List<Round>(3) { LastRound };
            Team1Points = 0;
            Team2Points = 0;
            GameValue = 1;
        }

        public void Start(List<Player> players) {
            
        }

        public void PlayCard(Player player, byte cardIndex, bool playedHidden) {
        }

        public void StartRound(bool isCangado) {
            LastRound = new Round();
            LastRound.IsCangado = isCangado;
            Rounds.Add(LastRound);
        }

        public void FinishRound(List<Player> players) {
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

        public void CallTruco() {
            IsTrucado = true;
        }

        public void AcceptTruco(bool isReturnTruco) {
            if (GameValue == 1) {
                GameValue = 3;
            }
            else {
                GameValue += 3;
            }
            if (!isReturnTruco) {
                IsTrucado = false;
            }
        }

        public void DeclineTruco(int playerTrucadoIndex) {
            IsTrucado = false;
            if (playerTrucadoIndex % 2 == 0) {
                Team1Points = 2;
            }
            else {
                Team2Points = 2;
            }
        }
    }
}
