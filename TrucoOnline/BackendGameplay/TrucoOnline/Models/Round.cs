namespace TrucoOnline.Models {
    public class Round {
        public Stack<PlayedCard> Cards { get; set; } = new Stack<PlayedCard>(4);
        public bool IsConcluded { get; set; }
        public bool IsCangado { get; set; }

        public PlayedCard GetRoundWinner() {
            var winners = new List<PlayedCard>();
            foreach (var card in Cards) {
                if (card.PlayedHidden) {
                    continue;
                }
                if (winners.Count == 0) {
                    winners.Add(card);
                    continue;
                }
                if (card.Card.Strength == winners.First().Card.Strength) {
                    if ((Array.IndexOf(Cards.ToArray(), card) + Array.IndexOf(Cards.ToArray(), winners.First())) % 2 == 0) {
                        continue;
                    }
                    winners.Add(card);
                }
                if (card.Card.Strength > winners.First().Card.Strength) {
                    winners.Clear();
                    winners.Add(card);
                }
            }
            if (winners.Count == 1) {
                return winners.First();
            }

            return null;
        }
    }
}
