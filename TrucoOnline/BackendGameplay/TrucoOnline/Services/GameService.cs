using TrucoOnline.Models;

namespace TrucoOnline.Services {
    public class GameService {
        public Lobby StartNewLobby() {
            var lobby = new Lobby();
            var player1 = new Player() { DisplayName = "Player 1" };
            var player2 = new Player() { DisplayName = "Player 2" };
            var player3 = new Player() { DisplayName = "Player 3" };
            var player4 = new Player() { DisplayName = "Player 4" };
            lobby.Players.Add(player1);
            lobby.Players.Add(player2);
            lobby.Players.Add(player3);
            lobby.Players.Add(player4);

            lobby.StartGame();
            return lobby;
        }
    }
}
