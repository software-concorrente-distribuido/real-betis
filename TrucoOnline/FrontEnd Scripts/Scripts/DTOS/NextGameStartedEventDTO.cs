using System.Collections.Generic;
using TrucoOnline.Models;

public class NextGameStartedEventDTO {
    public Game Game { get; set; }
    public List<Player> Players { get; set; }
    public Game PreviousGame { get; set; }
}
