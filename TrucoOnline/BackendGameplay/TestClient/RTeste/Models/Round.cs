using System;
using System.Collections.Generic;
using System.Text;

namespace RTeste.Models {
    public class Round {
        public Stack<PlayedCard> Cards { get; set; } = new Stack<PlayedCard>(4);
        public bool IsConcluded { get; set; }
    }
}
