using System;
using System.Collections.Generic;
using System.Text;

namespace WFTrucoTestClient.Models {
    public class Card {
        public string Value { get; set; }
        public CardSuit Suit { get; set; }
        public int Strength { get; set; }
    }
}
