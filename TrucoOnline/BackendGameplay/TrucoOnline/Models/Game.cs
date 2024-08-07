﻿namespace TrucoOnline.Models {
    public class Game {
        public Guid Id { get; set; }
        public Deck Deck { get; set; }
        public List<Round> Rounds { get; set; }
        public Round LastRound { get; set; }
        public Card? LastPlayedCard { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public bool IsGameFinished { get; set; }
        public bool IsTrucado { get; set; }
        public Guid? PlayerTrucadoId { get; set; }
        public byte GameValue { get; set; }

        public Game() {
            Id = Guid.NewGuid();
            Deck = new Deck();
            LastRound = new Round();
            Rounds = new List<Round>(3) { LastRound };
            Team1Points = 0;
            Team2Points = 0;
            GameValue = 1;
        }

        public void Start(List<Player> players) {
            foreach (var player in players) {
                player.Cards.Clear();
                for (var i = 0; i < 3; i++) {
                    player.Cards.Add(Deck.Cards.First());
                    Deck.Cards.RemoveAt(0);
                }
            }
        }

        public void PlayCard(Player player, byte cardIndex, bool playedHidden) {
            //TODO: SE TIVER VENCEDOR NO PRIMEIRO ROUND E CANGAR ENCERRAR O GAME
            if (LastRound.IsCangado) {
                var highestStrength = player.Cards.Max(c => c.Strength);
                cardIndex = (byte)player.Cards.FindIndex(c => c.Strength == highestStrength);
                playedHidden = false;
            }
            var card = player.Cards[cardIndex];
            player.Cards.RemoveAt(cardIndex);
            LastPlayedCard = card;

            LastRound.Cards.Push(new PlayedCard(card, player, playedHidden));
        }

        public void StartRound(bool isCangado) {
            LastRound = new Round();
            LastRound.IsCangado = isCangado;
            Rounds.Add(LastRound);
        }

        public void FinishRound(List<Player> players) {
            var winner = LastRound.GetRoundWinner();
            if (winner is not null) {
                if (players.IndexOf(winner.Player) % 2 == 0) {
                    if (LastRound.IsCangado) {
                        Team1Points = 2;
                    }
                    else {
                        Team1Points++;
                    }
                }
                else {
                    if (LastRound.IsCangado) {
                        Team2Points = 2;
                    }
                    else {
                        Team2Points++;
                    }
                }
            }
            else
            {
                if (Rounds.Count > 1)
                {
                    Round FirstRound = Rounds.First();
                    if (!FirstRound.IsCangado) 
                    {
                        if (players.IndexOf(FirstRound.GetRoundWinner().Player) % 2 == 0)
                        {
                            Team1Points = 2;
                        }
                        else Team2Points = 2;
                    }
                    else
                    {
                        if (Rounds.Count > 2) 
                        {
                            Round SecondRound = Rounds[1];
                            if (!SecondRound.IsCangado)
                            {
                                if (players.IndexOf(SecondRound.GetRoundWinner().Player) % 2 == 0)
                                {
                                    Team1Points = 2;
                                }
                                else Team2Points = 2;
                            }
                            else
                            {
                                IsGameFinished = true;
                            }
                        }
                    }
                }
            }

            if (Team1Points == 2 || Team2Points == 2 || Rounds.Count == 3) {
                IsGameFinished = true;
            }
        }

        public bool IsLastRoundFinished() {
            if (LastRound.Cards.Count == 4) {
                LastRound.IsConcluded = true;
            }

            return LastRound.IsConcluded;
        }

        public byte? GetGameWinner() {
            if (Team1Points == 2) {
                return 1;
            }
            else if (Team2Points == 2) {
                return 2;
            }

            return null;
        }

        public void CallTruco() {
            IsTrucado = true;
        }

        public void AcceptTruco(bool isReturnTruco) {
            if (GameValue == 1) {
                GameValue = 3;
            }
            else {
                GameValue += 3;
            }
            if (!isReturnTruco) {
                IsTrucado = false;
            }
        }

        public void DeclineTruco(int playerTrucadoIndex) {
            IsTrucado = false;
            if (playerTrucadoIndex % 2 == 0) {
                Team1Points = 2;
            }
            else {
                Team2Points = 2;
            }
        }
    }
}
