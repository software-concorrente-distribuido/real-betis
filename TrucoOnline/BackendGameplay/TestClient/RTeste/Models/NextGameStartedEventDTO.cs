using System.Collections.Generic;

namespace RTeste.Models {
    public class NextGameStartedEventDTO {
        public Game Game { get; set; }
        public List<Player> Players { get; set; }
    }
}
