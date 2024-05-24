using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;
using WFTrucoTestClient.Models;

namespace WFTrucoTestClient {
    public partial class Form1 : Form {
        public string PlayerName { get; set; }
        private HubConnection? _connection;
        private Guid? _lobbyId;
        private object gameLock = new object();
        private Lobby lobby = null;
        private Guid myPlayerId;
        private List<Button> monteButtons = new List<Button>();
        private List<Button> myCardsButtons = new List<Button>();
        private object myCardButtonsLock = new object();
        private bool isMyPlayerAdmin = false;
        private bool isMyTurn = false;

        public Form1() {
            InitializeComponent();
            monteButtons.Add(monteCard1);
            monteButtons.Add(monteCard2);
            monteButtons.Add(monteCard3);
            monteButtons.Add(monteCard4);
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private async void connectToServerButton_Click(object sender, EventArgs e) {
            string url = "http://" + hostServerTextBox.Text + ":4000/gameHub";

            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            await _connection.StartAsync();

            serverInfoText.Text = "Conectado";
            serverInfoText.ForeColor = Color.Green;

            connectToServerButton.Enabled = false;
            hostServerTextBox.Enabled = false;


            LoadConnectionEventHandlers();
        }

        private async void enterLobbyButton_Click(object sender, EventArgs e) {
            _lobbyId = Guid.Parse(lobbyIdTextBox.Text);
            EnterNameDialogForm enterNameDialogForm = new EnterNameDialogForm(this);
            enterNameDialogForm.ShowDialog();
            await EnterLobby();
        }

        private async Task EnterLobby() {
            await _connection!.InvokeAsync("ConnectGuestPlayerToLobby", _lobbyId, PlayerName);
        }

        private void LoadConnectionEventHandlers() {
            _connection!.On<CardPlayedEventDTO>("CardPlayed", param => {
                lock (gameLock) {
                    var button = monteButtons.First(b => b.Text == "");
                    SetText(GetCardText(param.PlayedCard), button);

                    lobby.Games.Last().CurrentPlayerIndex = param.CurrentPlayer;
                    lobby.Games.Last().LastRound.Cards.Push(new PlayedCard() { Card = param.PlayedCard, Player = lobby.Players.Find(p => p.Id == param.PlayerId) });
                    isMyTurn = myPlayerId == lobby.Players[param.CurrentPlayer].Id;
                    HandleIsMyTurn();
                }
            });

            _connection!.On<RoundStartedEventDTO>("RoundStarted", param => {
                lock (gameLock) {

                    if (param.LastRoundWinner != null) {
                        Console.WriteLine("Last Round Winner: " + param.LastRoundWinner.Player.DisplayName + " with " + param.LastRoundWinner.Card.Value + " of " + param.LastRoundWinner.Card.Suit);
                    }

                    if (param.NewRound.IsCangado) {
                        cangadoLabel.Enabled = true;
                    }
                    else {
                        cangadoLabel.Enabled = false;
                    }

                    SetText(param.Team1Points.ToString() + " x " + param.Team2Points.ToString(), roundScoreLabel);
                    monteButtons.ForEach(b => SetText("", b));
                    lobby.Games.Last().Rounds.Add(param.NewRound);
                    lobby.Games.Last().LastRound = param.NewRound;
                }
            });

            _connection!.On<GameFinishedEventDTO>("GameFinished", param => {
                lock (gameLock) {
                    SetText("Time 1: " + param.Team1Points.ToString(), team1PointsLabel);
                    SetText("Time 2: " + param.Team2Points.ToString(), team2PointsLabel);
                    SetText("0 x 0", roundScoreLabel);
                    monteButtons.ForEach(b => SetText("", b));
                }
            });

            _connection!.On<Lobby>("LobbySubscribed", param => {
                lock (gameLock) {
                    lobby = param;
                }
            });

            _connection!.On<NextGameStartedEventDTO>("NextGameStarted", param => {
                lock (gameLock) {
                    lobby.Games.Add(param.Game);
                    lobby.Players = param.Players;
                    var myPlayer = lobby.Players.Find(p => p.Id == myPlayerId);
                    myCardsButtons.Clear();
                    myCardsButtons.Add(card1Button);
                    myCardsButtons.Add(card2Button);
                    myCardsButtons.Add(card3Button);
                    SetText(GetCardText(myPlayer.Cards[0]), myCardsButtons[0]);
                    SetText(GetCardText(myPlayer.Cards[1]), myCardsButtons[1]);
                    SetText(GetCardText(myPlayer.Cards[2]), myCardsButtons[2]);
                    SetFormObjectVisibility(true, myCardsButtons[0]);
                    SetFormObjectVisibility(true, myCardsButtons[1]);
                    SetFormObjectVisibility(true, myCardsButtons[2]);
                    SetFormObjectVisibility(true, roundScoreLabel);
                    isMyTurn = myPlayerId == lobby.Players[param.Game.CurrentPlayerIndex].Id;
                    HandleIsMyTurn();
                }
            });

            _connection!.On<PlayerConnectedEventDTO>("PlayerConnected", param => {
                lock (gameLock) {
                    if (param.PlayerId == myPlayerId)
                        return;

                    Player player = new Player() { Id = param.PlayerId, DisplayName = param.DisplayName, IsLobbyAdmin = param.IsLobbyAdmin };
                    if (isMyPlayerAdmin && lobby.Players.Count == 4) {
                        startLobbyButton.Enabled = true;
                    }
                    lobby.Players.Add(player);
                    var playerListText = "";
                    foreach (var playerName in lobby.Players.Select(p => p.DisplayName).ToList()) {
                        playerListText += playerName + "\n";
                    }
                    SetText(playerListText, playersListLabel);
                }
            });

            _connection!.On<PlayerDisconnectedEventDTO>("PlayerDisconnected", param => {
                lock (gameLock) {
                    Console.WriteLine("****** PLAYER DISCONNECTED ******");
                    Console.WriteLine("\n");
                    lobby.Players.Remove(lobby.Players.Find(p => p.Id == param.PlayerId));
                }
            });

            _connection!.On<SelfPlayerConnectedEventDTO>("SelfPlayerConnected", param => {
                lock (gameLock) {
                    lobby = param.Lobby;
                    myPlayerId = param.PlayerId;
                }
                if (lobby.Players.Find(p => p.IsLobbyAdmin).Id == myPlayerId) {
                    isMyPlayerAdmin = true;
                    if (lobby.Players.Count == 4) {
                        startLobbyButton.Enabled = true;
                    }
                    SetFormObjectVisibility(true, startLobbyButton);
                }
                var playerListText = "";
                foreach (var playerName in lobby.Players.Select(p => p.DisplayName).ToList()) {
                    playerListText += playerName + "\n";
                }
                SetText(playerListText, playersListLabel);
                SetFormObjectVisibility(true, playersListLabel);
            });
        }

        delegate void SetTextCallback(string text, Control label);
        private void SetText(string text, Control label) {
            if (label.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text, label });
            }
            else {
                label.Text = text;
            }
        }

        delegate void SetFormObjectVisibilityCallback(bool visibility, Control obj);
        private void SetFormObjectVisibility(bool visibility, Control obj) {
            if (obj.InvokeRequired) {
                SetFormObjectVisibilityCallback d = new SetFormObjectVisibilityCallback(SetFormObjectVisibility);
                Invoke(d, new object[] { visibility, obj });
            }
            else {
                obj.Visible = visibility;
            }
        }

        delegate void SetFormObjectEnabledCallback(bool enabled, Control obj);
        private void SetFormObjectEnabled(bool enabled, Control obj) {
            if (obj.InvokeRequired) {
                SetFormObjectEnabledCallback d = new SetFormObjectEnabledCallback(SetFormObjectEnabled);
                Invoke(d, new object[] { enabled, obj });
            }
            else {
                obj.Enabled = enabled;
            }
        }

        private async void startLobbyButton_Click(object sender, EventArgs e) {
            await _connection!.InvokeAsync("StartLobby", _lobbyId, myPlayerId);
            SetFormObjectEnabled(false, startLobbyButton);
        }

        private async Task HandleCardClick(Button button) {
            await _connection!.InvokeAsync("PlayCard", _lobbyId, myPlayerId, myCardsButtons.FindIndex(b => b == button), false);
            SetFormObjectVisibility(false, button);
            myCardsButtons.Remove(button);
        }

        private async void card1Button_Click(object sender, EventArgs e) {
            await HandleCardClick(card1Button);
        }

        private async void card2Button_Click(object sender, EventArgs e) {
            await HandleCardClick(card2Button);
        }

        private async void card3Button_Click(object sender, EventArgs e) {
            await HandleCardClick(card3Button);
        }

        private string GetCardText(Card card) {
            if (card.Suit == CardSuit.Hidden) {
                return "ESCURO";
            }
            else if (card.Value == "4") {
                return "ZAP";
            }
            else if (card.Value == "A" && card.Suit == CardSuit.Spades) {
                return "ESPADILHA";
            }

            var cardText = card.Value + " de ";
            switch (card.Suit) {
                case CardSuit.Clubs:
                    cardText += "PAUS";
                    break;
                case CardSuit.Diamonds:
                    cardText += "OUROS";
                    break;
                case CardSuit.Hearts:
                    cardText += "COPAS";
                    break;
                case CardSuit.Spades:
                    cardText += "ESPADAS";
                    break;
            }

            return cardText;
        }
        private void HandleIsMyTurn() {
            foreach (var myCardButton in myCardsButtons.ToList()) {
                SetFormObjectEnabled(isMyTurn, myCardButton);
            }
        }
    }
}
