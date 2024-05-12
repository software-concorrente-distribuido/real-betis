namespace TrucoOnline.Models {
    public class Lobby {
        public Guid Id { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
