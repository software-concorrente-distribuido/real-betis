using System;
using System.Collections.Generic;

namespace TrucoOnline.Models {
    public class Round {
        public Stack<PlayedCard> Cards { get; set; } = new Stack<PlayedCard>(4);
        public bool IsConcluded { get; set; }
        public bool IsCangado { get; set; }
    }
}
