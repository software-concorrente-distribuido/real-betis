using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;
using TrucoOnline.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance {get; private set;}
    public static string URL = "";

    HubConnection _connection;
    //TODO RETIRAR O PUBLIC DEPOIS DE DEBUGAR
    public Lobby lobby = null;
    public Guid myPlayerId;
    public int myPlayerIndex;
    public string myPlayerName = null;
    public bool isMyPlayerAdmin;
    private bool isMyTurn;
    public int trucosCalled = 0;
    private bool connectedToLobby = false;

    void Start()
    {
        Application.runInBackground = true;
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public async Task  StartConnection(){
       String url = URL + "/gamehub";

        _connection = new HubConnectionBuilder()
        .WithUrl(url)
        .WithAutomaticReconnect()
        .Build();

        await _connection.StartAsync();

        LoadConnectionEventHandlers();
    }

    private void LoadConnectionEventHandlers() {
        _connection!.On<SelfPlayerConnectedEventDTO>("SelfPlayerConnected", param => {
            lobby = param.Lobby;
            myPlayerId = param.PlayerId;
            connectedToLobby = true;

            LobbyManager.Instance.ToggleLeaveLobbyButton(true);

            if (lobby.Players.Find(p => p.IsLobbyAdmin).Id == myPlayerId) {
                isMyPlayerAdmin = true;
            }

            if (lobby.Players.Count == 4) {
                SceneManager.LoadScene("GameScene");
            }
        });

        _connection!.On<PlayerConnectedEventDTO>("PlayerConnected", param => {
            if (param.PlayerId == myPlayerId)
                return;

            Player player = new() { Id = param.PlayerId, DisplayName = param.DisplayName, IsLobbyAdmin = param.IsLobbyAdmin, LobbyIndex=param.LobbyIndex };

            lobby.Players.Add(player);
            lobby.Players = lobby.Players.OrderBy(p => p.LobbyIndex).ToList();

            //TODO: VER COMO FAZER PARA ATUALIZAR TODOS OS LOBBIES SEMPRE QUE TIVER ALTERACAO (?) - SIM OU NAO ??

            LobbyManager.Instance.UpdateOwnLobbyStatusOnCallback(lobby.Id, lobby.Players);
            
            if(lobby.Players.Count == 4){
                SceneManager.LoadScene("GameScene");
            }
            
        });

        _connection!.On<PlayerDisconnectedEventDTO>("PlayerDisconnected", param => {
            lobby.Players.Remove(lobby.Players.Find(p => p.Id == param.PlayerId));
            bool myPlayerDisconnected = param.PlayerId == myPlayerId;


            LobbyManager.Instance.RemovePlayerFromLobby(lobby.Id, param.LobbyIndex, myPlayerDisconnected);

            if(param.PlayerId == myPlayerId){
                LobbyManager.Instance.ToggleLeaveLobbyButton(false);
                connectedToLobby = false;
            }
        });

        _connection!.On("AllPlayersDisconnected", () => {
            connectedToLobby = false;
        });

        _connection!.On<NextGameStartedEventDTO>("NextGameStarted", param => {
            lobby.Games.Add(param.Game);
            lobby.Players = param.Players;
            var myPlayer = lobby.Players.Find(p => p.Id == myPlayerId);
            myPlayerIndex = lobby.Players.IndexOf(myPlayer);

            trucosCalled = 0;
            
            isMyTurn = myPlayerId == lobby.Players[param.Game.CurrentPlayerIndex].Id;

            if(myPlayer.Cards != null && myPlayer.Cards.Count > 0){
                //TODO instanciar baseado no index dos jogadores
                StartCoroutine(GameManager.Instance.InstantiateCards(lobby.Players, myPlayer.Cards, isMyTurn));
            }
        });

        _connection!.On<CardPlayedEventDTO>("CardPlayed", param => {
            if(myPlayerId != lobby.Players[lobby.Games.Last().CurrentPlayerIndex].Id){
                Deck.Instance.RemoveCardTransformFromPlayerHand(lobby.Players[lobby.Games.Last().CurrentPlayerIndex].Id);
                Deck.Instance.InstantiatePlayedCardOnDropZone(param.PlayedCard);
            }

            lobby.Games.Last().CurrentPlayerIndex = param.CurrentPlayer;
            lobby.Games.Last().LastRound.Cards.Push(new PlayedCard() { Card = param.PlayedCard, Player = lobby.Players.Find(p => p.Id == param.PlayerId) });
            
            isMyTurn = myPlayerId == lobby.Players[param.CurrentPlayer].Id;
            HandleIsMyTurn();
        });

        _connection!.On<RoundStartedEventDTO>("RoundStarted", param => {
            if(param.LastRoundWinner != null){
                StartCoroutine(ShowLastWinner(param.LastRoundWinner)); 
            }

            if(param.NewRound.IsCangado){
                GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "O round esta cangado!";
            }

            Counter.Instance.team1 = param.Team1Points;
            Counter.Instance.team2 = param.Team2Points;

            StartCoroutine(GameManager.Instance.RemoveDropZoneCards(true));
            
            lobby.Games.Last().Rounds.Add(param.NewRound);
            lobby.Games.Last().LastRound = param.NewRound;
        });


        _connection!.On<GameFinishedEventDTO>("GameFinished", param => {
            MatchScore.Instance.team1 = param.Team1Points;
            MatchScore.Instance.team2 = param.Team2Points;  

            if(param.Team1Points == 12 || param.Team2Points == 12){
                StartCoroutine(ShowGameWinner(param));
            }
            else{
                Counter.Instance.RestartScore();
                GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "";
                StartCoroutine(GameManager.Instance.RemoveDropZoneCards(false));
            }
            
        });

        _connection!.On<TrucoCalledEventDTO>("TrucoCalled", param => {
            trucosCalled++;
            
            GameManager.Instance.HandleTrucoCall(trucosCalled, param.playerRequestedIndex);

            switch(trucosCalled){
                case 1:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "TRUCO!";
                    break;
                case 2:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "SEIS!";
                    break;
                case 3:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "NOVE!";
                    break;
                case 4:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "DOZE!";
                    break;
            }

            if(myPlayerId.Equals(param.playerTrucadoId)){
                GameManager.Instance.HandlePlayerTrucado(trucosCalled);
            }
        });

        _connection!.On("TrucoAccepted", () => {
            Player currentPlayer = lobby.Players[lobby.Games.Last().CurrentPlayerIndex];

             switch(trucosCalled){
                case 1:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "TRUCO ACEITO!";
                    break;
                case 2:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "TRUCO ACEITO - SEIS!";
                    break;
                case 3:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "TRUCO ACEITO - NOVE!";
                    break;
                case 4:
                    GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "TRUCO ACEITO - DOZE!";
                    break;
            }

            if(currentPlayer.Id.Equals(myPlayerId)){
                Deck.Instance.HandleIsMyTurn(true);
            }

        });

    }

    private void HandleIsMyTurn() {
        Deck.Instance.HandleIsMyTurn(isMyTurn);
    }

    public async Task ConnectToLobby(Guid lobbyId, int indexPos){
        if(connectedToLobby) await ExitLobby();
        await _connection!.InvokeAsync("ConnectGuestPlayerToLobby", lobbyId, myPlayerName, indexPos);
    }

    public async Task ExitLobby(){
        isMyPlayerAdmin = false;
        await _connection!.InvokeAsync("DisconnectPlayerFromLobby", lobby.Id, myPlayerId);
        await _connection!.InvokeAsync("UnsubscribeFromLobby", lobby.Id);
    }

    public async Task DisconnectAllPlayers(){
        await _connection!.InvokeAsync("DisconnectAllPlayers", lobby.Id);
        await _connection!.InvokeAsync("UnsubscribeFromLobby", lobby.Id);
    }

    public async Task UnsubscribeFromLobby(){
        await _connection!.InvokeAsync("UnsubscribeFromLobby", lobby.Id);
    }

    public async void StartLobby(){
        await _connection!.InvokeAsync("StartLobby", lobby.Id, myPlayerId);
    }

    public async Task CallTruco(){
        Player currentPlayer = lobby.Players[lobby.Games.Last().CurrentPlayerIndex];

        await _connection!.InvokeAsync("CallTruco", lobby.Id, currentPlayer.Id);
    }

    public async Task ReturnCallTruco(){
        await _connection!.InvokeAsync("ReturnCallTruco", lobby.Id, myPlayerId);
    }

    public async Task AcceptTruco(){
        await _connection!.InvokeAsync("AcceptTruco", lobby.Id, myPlayerId);
    }

    public async Task DeclineTruco(){
        await _connection!.InvokeAsync("DeclineTruco", lobby.Id, myPlayerId);
    }

    public async void PlayCard(Card card) {
        var myPlayer = lobby.Players.Find(p => p.Id == myPlayerId);
        
        await _connection!.InvokeAsync("PlayCard", lobby.Id, myPlayerId,  myPlayer.Cards.FindIndex(c => c.Value == card.Value && c.Suit == card.Suit), false);
        
        myPlayer.Cards.RemoveAt(myPlayer.Cards.FindIndex(c => c.Value == card.Value && c.Suit == card.Suit));
    }

    private IEnumerator ShowLastWinner(PlayedCard LastRoundWinner){
        Player lastWinner = lobby.Players.Find(p => p.Id == LastRoundWinner.Player.Id);
        GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "Ganhador do round: " + lastWinner.DisplayName + " com " + GetCardText(LastRoundWinner.Card);

        yield return new WaitForSeconds(4f);

        GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "";
    }

    private IEnumerator ShowLastRoundWinner(byte? WinnerTeam){
        if(WinnerTeam == 1) GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "Ganhador da rodada: Time 1";
        else GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "Ganhador da rodada: Time 2";

        yield return new WaitForSeconds(4f);

        GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "";
    }

    private IEnumerator ShowGameWinner(GameFinishedEventDTO gamePoints){
        if(gamePoints.Team1Points == 12) GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "Vencedor da partida: Time 1 por " + gamePoints.Team1Points + " x " + gamePoints.Team2Points;
        else GameObject.Find("LastRoundInfo").GetComponent<TMP_Text>().text = "Vencedor da partida: Time 2 por " + gamePoints.Team2Points + " x " + gamePoints.Team1Points;
        
        yield return new WaitForSeconds(4f);

        _ = DisconnectAllPlayers();
        SceneManager.LoadScene("LobbyScene");
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

    void OnApplicationQuit()
    {
        if(lobby != null && myPlayerId != null){
            _ = ExitLobby();
        }
    }
}
