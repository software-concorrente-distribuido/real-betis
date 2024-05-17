namespace RTeste.Models {
    public class PlayedCard {
        public Card Card { get; set; }
        public Player Player { get; set; }
        public bool PlayedHidden { get; set; }
    }
}
