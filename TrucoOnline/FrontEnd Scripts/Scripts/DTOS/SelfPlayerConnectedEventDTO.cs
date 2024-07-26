using System;
using TrucoOnline.Models;

public class SelfPlayerConnectedEventDTO {
    public Lobby Lobby { get; set; }
    public Guid PlayerId { get; set; }
}
