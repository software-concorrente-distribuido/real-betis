namespace WFTrucoTestClient.Models {
    public class RoundStartedEventDTO {
        public Round NewRound { get; set; }
        public PlayedCard LastRoundWinner { get; set; }
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
    }
}
