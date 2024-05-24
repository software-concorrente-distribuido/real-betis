using System;

namespace WFTrucoTestClient.Models {
    public class SelfPlayerConnectedEventDTO {
        public Lobby Lobby { get; set; }
        public Guid PlayerId { get; set; }
    }
}
