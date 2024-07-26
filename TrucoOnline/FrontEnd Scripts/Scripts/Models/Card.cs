using System.Collections.Generic;

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

        public static Card CardObjectToCard(CardObject cardObject)
        {
            string Value = cardObject.Value;
            int Strength = cardObject.Strength;
            CardSuit CardSuit;

            Dictionary<string, CardSuit> dictionary = new Dictionary<string, CardSuit>(){
                {"Clubs", CardSuit.Clubs},
                {"Spades", CardSuit.Spades},
                {"Diamonds", CardSuit.Diamonds},
                {"Hearts", CardSuit.Hearts},
            };

            CardSuit = dictionary[cardObject.Suit];

            return new Card(Value, CardSuit, Strength);
        }
    }
}
