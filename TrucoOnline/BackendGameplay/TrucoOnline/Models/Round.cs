namespace TrucoOnline.Models {
    public class Round {
        public Stack<PlayedCard> Cards { get; set; } = new Stack<PlayedCard>(4);
        public bool IsConcluded { get; set; }
        public bool IsCangado { get; set; }

        public PlayedCard GetRoundWinner() {
            var winner = new PlayedCard(new Card("", CardSuit.Clubs, 0), null);
            foreach (var card in Cards) {
                if (card.Card.Strength == winner.Card.Strength) {
                    return null;
                }
                if (card.Card.Strength > winner.Card.Strength) {
                    winner = card;
                }
            }
            return winner;
        }
    }
}
