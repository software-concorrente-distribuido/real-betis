namespace TrucoOnline.Models {
    public class PlayedCard {
        public Card Card { get; set; }
        public Player Player { get; set; }
        public bool PlayedHidden { get; set; }

        public PlayedCard(Card card, Player player) {
            Card = card;
            Player = player;
        }
    }
}
