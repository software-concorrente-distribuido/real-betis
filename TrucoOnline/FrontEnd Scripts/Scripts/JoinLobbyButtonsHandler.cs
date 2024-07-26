using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TrucoOnline.Models;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyButtonsHandler : MonoBehaviour
{
    public GameObject Player0Join;
    public GameObject Player1Join;
    public GameObject Player2Join;
    public GameObject Player3Join;

    Dictionary<int, GameObject> IndexToGameObject = null;

    public async void HandlePlayer0(){
        DisableOtherButtons();
        
        JoinLobbyArt parentScript = transform.parent.gameObject.GetComponent<JoinLobbyArt>();
        await GlobalManager.Instance.ConnectToLobby(new Guid(parentScript.Id), 0);
        
        TMP_Text buttonText = Player0Join.GetComponentInChildren<TMP_Text>();
        buttonText.text = GlobalManager.Instance.myPlayerName;
    }

    public async void HandlePlayer1(){
        DisableOtherButtons();
        
        JoinLobbyArt parentScript = transform.parent.gameObject.GetComponent<JoinLobbyArt>();
        await GlobalManager.Instance.ConnectToLobby(new Guid(parentScript.Id), 1);
        
        TMP_Text buttonText = Player1Join.GetComponentInChildren<TMP_Text>();
        buttonText.text = GlobalManager.Instance.myPlayerName;
    }

    public async void HandlePlayer2(){
        DisableOtherButtons();
        
        JoinLobbyArt parentScript = transform.parent.gameObject.GetComponent<JoinLobbyArt>();
        await GlobalManager.Instance.ConnectToLobby(new Guid(parentScript.Id), 2);
        
        TMP_Text buttonText = Player2Join.GetComponentInChildren<TMP_Text>();
        buttonText.text = GlobalManager.Instance.myPlayerName;
    }

    public async void HandlePlayer3(){
        DisableOtherButtons();
        JoinLobbyArt parentScript = transform.parent.gameObject.GetComponent<JoinLobbyArt>();
        await GlobalManager.Instance.ConnectToLobby(new Guid(parentScript.Id), 3);
        
        TMP_Text buttonText = Player3Join.GetComponentInChildren<TMP_Text>();
        buttonText.text = GlobalManager.Instance.myPlayerName;
    }

    private void EnableAllButtons(){
        Player0Join.GetComponent<Button>().enabled = true;
        Player1Join.GetComponent<Button>().enabled = true;
        Player2Join.GetComponent<Button>().enabled = true;
        Player3Join.GetComponent<Button>().enabled = true;
    }

    private void DisableOtherButtons(){
        Player0Join.GetComponent<Button>().enabled = false;
        Player1Join.GetComponent<Button>().enabled = false;
        Player2Join.GetComponent<Button>().enabled = false;
        Player3Join.GetComponent<Button>().enabled = false;
    }

    void SetPlayerInButton(Player player){
        GameObject go = IndexToGameObject[player.LobbyIndex];

        TMP_Text buttonText = go.GetComponentInChildren<TMP_Text>();
        buttonText.text = player.DisplayName;
        go.GetComponent<Button>().enabled = false;
    }

    public void InsertPlayersInLobby(List<Player> players){
        LoadDict();

        foreach(Player p in players){
            SetPlayerInButton(p);
        }
    }

    public void RestoreButtonOriginalState(int index, bool myPlayerDisconnected){
        LoadDict();
        GameObject go = IndexToGameObject[index];

        TMP_Text buttonText = go.GetComponentInChildren<TMP_Text>();
        buttonText.text = "+";
        go.GetComponent<Button>().enabled = true;

        if(myPlayerDisconnected) EnableAllButtons();
    }

    void LoadDict(){
        IndexToGameObject ??= new Dictionary<int, GameObject>(){
            {0, Player0Join},
            {1, Player1Join},
            {2, Player2Join},
            {3, Player3Join}
        };
    }
}
