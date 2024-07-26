using System;
using System.Collections.Generic;

namespace TrucoOnline.Models {
    [Serializable]
    public class Player {
        public Guid Id { get; set; }
        public String DisplayName { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>(3);
        public bool IsLobbyAdmin { get; set; }
        public int LobbyIndex {get; set;}

        public Player() {
            Id = Guid.NewGuid();
        }
    }
}
