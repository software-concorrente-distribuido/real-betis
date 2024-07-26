using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrucoOnline.Models;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Player myPlayer = new Player(new List<CardObject> (), 1);
    Player opponent1 = new Player(new List<CardObject> (), 2);
    Player teammate = new Player(new List<CardObject> (), 3);
    Player opponent2 = new Player(new List<CardObject> (), 4);

    public int TeamRequestTruco;
    public bool TrucoRequested;
    public int RoundPoints;

    public GameObject AcceptTrucoButton;
    public GameObject DeclineTrucoButton;
    public GameObject RecallTruco;
    public GameObject TrucoButton;

    public AudioSource AudioSource;
    public AudioClip AudioClip;
    public GameObject YourTurn;

    GlobalManager GlobalManager;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        GlobalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();

        TrucoButton.SetActive(false);
        DeactivateButtons();

        Deck.Instance.DeferPlayerIdToTransform(GlobalManager.lobby.Players, GlobalManager.myPlayerId);

        if(GlobalManager.isMyPlayerAdmin) {
            GameObject.Find("StartButton").SetActive(true);
        }
        else  GameObject.Find("StartButton").SetActive(false);
        
    }

    public void StartLobby(){
        GlobalManager.StartLobby();
        GameObject.Find("StartButton").SetActive(false);
    }
    public async void CallTruco(){
        TeamRequestTruco = GlobalManager.Instance.myPlayerIndex % 2;
        TrucoRequested = true;

        TrucoButton.SetActive(false);
        Deck.Instance.HandleIsMyTurn(false);

        await GlobalManager.Instance.CallTruco();
    }

    public void HandleTrucoCall (int trucosCalled ,int playerRequestedIndex){
        TeamRequestTruco = playerRequestedIndex % 2;
        TrucoRequested = true;
        
        switch(trucosCalled){
            case 1:
                TrucoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Seis!";
                break;
            case 2:
                TrucoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Nove!";
                break;
            case 3:
                TrucoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Doze!";
                break;
            case 4:
                TrucoButton.SetActive(false);
                break;
        }
    }

    public void HandlePlayerTrucado (int trucosCalled){
        ActivateButtons(trucosCalled);
    }

    void ActivateButtons(int trucosCalled){
        AcceptTrucoButton.SetActive(true);
        DeclineTrucoButton.SetActive(true);

        RecallTruco.SetActive(true);
        switch(trucosCalled){
            case 1:
                RecallTruco.GetComponentInChildren<TextMeshProUGUI>().text = "Seis!";
                break;
            case 2:
                RecallTruco.GetComponentInChildren<TextMeshProUGUI>().text = "Nove!";
                break;
            case 3:
                RecallTruco.GetComponentInChildren<TextMeshProUGUI>().text = "Doze!";
                break;
            case 4:
                RecallTruco.SetActive(false);
                break;
        }

    }

    void DeactivateButtons(){
        AcceptTrucoButton.SetActive(false);
        DeclineTrucoButton.SetActive(false);
        RecallTruco.SetActive(false);
    }

    public async void AcceptTrucoAction(){
        DeactivateButtons();
        await GlobalManager.Instance.AcceptTruco();
    }

    public async void DeclineTrucoAction(){
        DeactivateButtons();
        TrucoRequested = false;
        TeamRequestTruco = 3;
        await GlobalManager.Instance.DeclineTruco();
    }

    public async void RecallTrucoAction(){
        DeactivateButtons();
        TeamRequestTruco = GlobalManager.Instance.myPlayerIndex % 2;
        await GlobalManager.Instance.ReturnCallTruco();
    }

    void DisablePlayersHands(){
        foreach(CardObject card in opponent1.Hand) card.gameObject.GetComponent<Drag>().enabled = false;
        foreach(CardObject card in teammate.Hand) card.gameObject.GetComponent<Drag>().enabled = false;
        foreach(CardObject card in opponent2.Hand) card.gameObject.GetComponent<Drag>().enabled = false;
    }

    public IEnumerator DealOtherPlayersCards(bool isMyTurn){
        List<Player> orderDEBUG = new List<Player>{opponent1, teammate, opponent2};

        foreach(Player player in orderDEBUG){
            for(int i = 0; i<3; i++){
                player.Hand.Add(Deck.Instance.HandOutOtherPlayersCards(player.Id));
                AudioSource.PlayOneShot(AudioClip);
                yield return new WaitForSeconds(0.5f);
            }
        }

        DisablePlayersHands();
        Deck.Instance.HandleIsMyTurn(isMyTurn);
    }

    public void RemoveCardFromHand(CardObject cardToRemove){
        myPlayer.Hand.Remove(cardToRemove);
    }

    IEnumerator RemoveRemainingCards(){
        List<Player> players = new List<Player>(){myPlayer, opponent1, teammate, opponent2};
        players = players.OrderBy(p => p.LobbyIndex).ToList();

        int index = 0;
        foreach(Player p in players){
            Deck.Instance.RemoveRemainingCards(p.Id);
            p.Hand = new List<CardObject>();
            index++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator InstantiateCards(List<TrucoOnline.Models.Player> players, List<Card> cardsToInstantiate, bool isMyTurn){
        DeferLobbyIndexToPlayer();
        TrucoButton.GetComponentInChildren<TextMeshProUGUI>().text = "TRUCO!";
        StartCoroutine(RemoveRemainingCards());

        List<Player> GameManagerPlayers = new List<Player>(){myPlayer, opponent1, teammate, opponent2};

        foreach(TrucoOnline.Models.Player p in players){
            if(p.Id == GlobalManager.Instance.myPlayerId){
                foreach(Card cardToInstantiate in cardsToInstantiate){
                    myPlayer.Hand.Add(Deck.Instance.HandOutPlayerCard(cardToInstantiate));
                    AudioSource.PlayOneShot(AudioClip);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else {
                Player player = GameManagerPlayers.Find(pl => pl.LobbyIndex == p.LobbyIndex);
                for(int i = 0; i<3; i++){
                    player.Hand.Add(Deck.Instance.HandOutOtherPlayersCards(p.Id));
                    AudioSource.PlayOneShot(AudioClip);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        
        DisablePlayersHands();
        Deck.Instance.HandleIsMyTurn(isMyTurn);
        //StartCoroutine(DealOtherPlayersCards(isMyTurn));
    }

    private void DeferLobbyIndexToPlayer(){
        switch(GlobalManager.Instance.myPlayerIndex){
            case 0:
                myPlayer.LobbyIndex = 0;
                opponent1.LobbyIndex = 1;
                teammate.LobbyIndex = 2;
                opponent2.LobbyIndex = 3;
                break; 
            case 1:
                myPlayer.LobbyIndex = 1;
                opponent1.LobbyIndex = 2;
                teammate.LobbyIndex = 3;
                opponent2.LobbyIndex = 0;
                break; 
            case 2:
                myPlayer.LobbyIndex = 2;
                opponent1.LobbyIndex = 3;
                teammate.LobbyIndex = 0;
                opponent2.LobbyIndex = 1;
                break; 
            case 3:
                myPlayer.LobbyIndex = 3;
                opponent1.LobbyIndex = 0;
                teammate.LobbyIndex = 1;
                opponent2.LobbyIndex = 2;
                break; 
        }

        List<Player> players = new List<Player>(){myPlayer, opponent1, teammate, opponent2};

        foreach(TrucoOnline.Models.Player lobbyPlayer in GlobalManager.Instance.lobby.Players){
            Player p = players.Find(pl => pl.LobbyIndex == lobbyPlayer.LobbyIndex);
            p.Id = lobbyPlayer.Id;
        }
    }

    public IEnumerator RemoveDropZoneCards(bool sameGame){
        if(!sameGame){
            TrucoRequested = false;
            TeamRequestTruco = 3;
        }

        GameObject dropZone = GameObject.Find("DropZone");
        for (var i = dropZone.transform.childCount - 1; i >= 0; i--){
            yield return new WaitForSeconds(0.2f);
            Destroy(dropZone.transform.GetChild(i).gameObject);
        }

        TrucoButton.SetActive(false);
    }

    public async void ExitLobby(){
        await GlobalManager.Instance.ExitLobby();
        SceneManager.LoadScene("LobbyScene");
    }

    public class Player
    {
        public List<CardObject>  Hand;
        public int Index;
        public int LobbyIndex;
        public Guid Id;

        //pegar id do server pra ficar mais facil

        public Player(List<CardObject>  _hand, int _index){
            Hand = _hand;
            Index = _index;
        }
    }

}
