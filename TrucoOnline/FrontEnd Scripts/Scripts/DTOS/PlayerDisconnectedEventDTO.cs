using System;

public class PlayerDisconnectedEventDTO {
    public Guid PlayerId { get; set; }
    public int LobbyIndex { get; set; }
}
