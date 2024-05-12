using TrucoOnline.Models.Cards;

namespace TrucoOnline.Models {
    public class Deck {
        public List<Card> Cards { get; set; }

        public Deck() {
            Cards = new List<Card>() {
                new JClubs(),
                new JDiamonds(),
                new JHearts(),
                new JSpades(),
                new QClubs(),
                new QDiamonds(),
                new QHearts(),
                new QSpades(),
                new KClubs(),
                new KDiamonds(),
                new KHearts(),
                new KSpades(),
                new AClubs(),
                new ADiamonds(),
                new AHearts(),
                new _2Clubs(),
                new _2Diamonds(),
                new _2Hearts(),
                new _2Spades(),
                new _3Clubs(),
                new _3Diamonds(),
                new _3Hearts(),
                new _3Spades(),
                new SeteDeOuro(),
                new Espadilha(),
                new SeteDeCopas(),
                new Zap()
            };

            Cards = Cards.OrderBy(c => Random.Shared.Next()).ToList();
        }
    }
}
