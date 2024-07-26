using System;
using System.Collections.Generic;
using TMPro;
using TrucoOnline.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinLobbyArt : MonoBehaviour
{
    GlobalManager GlobalManager;
    LobbyManager LobbyManager;
    TMP_Text LobbyId;
    public String Id;
    public List<Player> Players;
    int Index;

    void Start() {
        GlobalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        LobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
    }

    void Awake(){
        LobbyId = GetComponentInChildren<TMP_Text>();
    }

    void Update(){
        LobbyId.text = "Lobby " + Index;
    }

    public void SetUpLobbyButton(String _id, List<Player> _players, int _index){
        Id = _id;
        Players = _players;
        Index = _index + 1;

        InsertPlayersInLobby();
    }

    public void UpdatePlayersInLobby(List<Player> players){
        Players = players;
        InsertPlayersInLobby();
    }

    public void RemovePlayerFromLobby(int lobbyIndex, bool myPlayerDisconnected){
        JoinLobbyButtonsHandler script = gameObject.transform.Find("ButtonHandler").GetComponent<JoinLobbyButtonsHandler>();
        script.RestoreButtonOriginalState(lobbyIndex, myPlayerDisconnected);
    }

    void InsertPlayersInLobby(){
        JoinLobbyButtonsHandler script = gameObject.transform.Find("ButtonHandler").GetComponent<JoinLobbyButtonsHandler>();
        script.InsertPlayersInLobby(Players);
    }
}
