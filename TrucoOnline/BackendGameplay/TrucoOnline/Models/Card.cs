namespace TrucoOnline.Models {
    public class Card {
        public string Value { get; set; }
        public CardSuit Suit { get; set; }
        public int Strength { get; set; }

        public Card(string value, CardSuit suit, int strength) {
            Value = value;
            Suit = suit;
            Strength = strength;
        }
    }
}
