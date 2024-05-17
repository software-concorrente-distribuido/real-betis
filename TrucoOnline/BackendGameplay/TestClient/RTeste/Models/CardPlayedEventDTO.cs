using System;

namespace RTeste.Models {
    public class CardPlayedEventDTO {
        public Guid PlayerId { get; set; }
        public Card PlayedCard { get; set; }
        public int CurrentPlayer { get; set; }
    }
}
