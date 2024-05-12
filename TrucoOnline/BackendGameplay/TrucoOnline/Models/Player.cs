namespace TrucoOnline.Models {
    public class Player {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>(3);

        public Player() {
            Id = Guid.NewGuid();
        }
    }
}
