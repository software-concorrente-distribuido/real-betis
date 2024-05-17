using System.Collections.Generic;
using System;

namespace RTeste.Models {
    public class Lobby {
        public Guid Id { get; set; }
        public List<Game> Games { get; set; }
        public List<Player> Players { get; set; }
        public byte Team1Points { get; set; }
        public byte Team2Points { get; set; }
    }
}
