using System.Collections.Generic;

namespace WFTrucoTestClient.Models {
    public class NextGameStartedEventDTO {
        public Game Game { get; set; }
        public List<Player> Players { get; set; }
    }
}
