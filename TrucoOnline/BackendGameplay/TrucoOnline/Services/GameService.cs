using TrucoOnline.Models;

namespace TrucoOnline.Services {
    public class GameService {
        public Game GetGame() {
            var game = new Game();
            var player1 = new Player() { DisplayName = "Player 1" };
            var player2 = new Player() { DisplayName = "Player 2" };
            var player3 = new Player() { DisplayName = "Player 3" };
            var player4 = new Player() { DisplayName = "Player 4" };
            game.Players.Add(player1);
            game.Players.Add(player2);
            game.Players.Add(player3);
            game.Players.Add(player4);

            game.Start();
            return game;
        }
    }
}
