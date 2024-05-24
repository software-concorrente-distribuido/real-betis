using System;

namespace WFTrucoTestClient.Models {
    public class PlayerConnectedEventDTO {
        public Guid PlayerId { get; set; }
        public string DisplayName { get; set; }
        public bool IsLobbyAdmin { get; set; }
    }
}
