using System;
using System.Collections.Generic;

namespace RTeste.Models {
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
    }
}
