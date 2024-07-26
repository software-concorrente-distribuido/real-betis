using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TrucoOnline.Models;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    List<LobbyDTO> lobbyList = new List<LobbyDTO>();
    public GameObject LobbyArt;
    [SerializeField] Transform LobbyZone;
    List<GameObject> lobbiesOnScreen;
    public GameObject ModalPanel;
    public GameObject RefreshButton;
    public GameObject CreateLobbyButton;
    public GameObject NicknameText;
    public GameObject ScrollView;
    public GameObject LeaveLobbyButton;
    public GameObject MainMenuButton;

    //TODO
    //mostrar o vencedor da ultima rodada
    //rodar as maos depois que encerrar o game
    //atualizar o feed de lobby sem refresh

    void Start()
    {
        if(GlobalManager.Instance.myPlayerName.Equals(null) || GlobalManager.Instance.myPlayerName.Equals("")){
            ToggleModal(true);
        } 
        else{ 
            NicknameText.GetComponent<TMP_Text>().text = GlobalManager.Instance.myPlayerName;
            InitAfterNickname();
        }
    }

    void Awake(){
        Instance = this;
    }

    void ToggleModal(bool toggle){
        ModalPanel.SetActive(toggle);
        RefreshButton.SetActive(!toggle);
        CreateLobbyButton.SetActive(!toggle);
        NicknameText.SetActive(!toggle);
        ScrollView.SetActive(!toggle);
        MainMenuButton.SetActive(!toggle);
    }

    void InitAfterNickname(){
        lobbiesOnScreen = new List<GameObject>();
        StartCoroutine(FindAllLobbies());
    }

    public void Clique()
    {
        StartCoroutine(CriarLobby());
    }

    public void LoadLobbies(){
        StartCoroutine(FindAllLobbies());
    }

    public void ToggleLeaveLobbyButton(bool toggle){
        LeaveLobbyButton.SetActive(toggle);
    }

    public async void LeaveLobby(){
        await GlobalManager.Instance.ExitLobby();
    }

    public async void MenuButton(){
        if(GlobalManager.Instance.lobby != null){
            await GlobalManager.Instance.ExitLobby();
        }

        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator FindAllLobbies(){
        using (UnityWebRequest Request = UnityWebRequest.Get(GlobalManager.URL + "/game/lobbies"))
        {
            yield return Request.SendWebRequest();

            if(Request.result == UnityWebRequest.Result.Success){
                var response = Request.downloadHandler.text;

                LobbyDTO[] lista = JsonHelper.getJsonArray<LobbyDTO>(response);

                lobbyList = lista.ToList();

                //PENSAR EM UMA MANEIRA MELHOR DE FAZER ISSO
                DestroyLobbyObjects();
                InstantiateLobbyObjects();
            }

        };
    }

    void DestroyLobbyObjects(){
        if(lobbiesOnScreen.Count > 0){
            lobbiesOnScreen[0].GetComponent<Transform>().parent.DetachChildren();
            foreach(GameObject obj in lobbiesOnScreen) Destroy(obj);
            lobbiesOnScreen = new List<GameObject>();
        }
    }

    void InstantiateLobbyObjects(){
        foreach(LobbyDTO lobby in lobbyList){
            GameObject newLobby = Instantiate(LobbyArt);
            JoinLobbyArt joinLobbyArt = newLobby.GetComponent<JoinLobbyArt>();
            
            List<Player> playerList = new List<Player>();
            foreach(PlayerDTO pDTO in lobby.players){
                Player player = new() { Id = new Guid(pDTO.id), DisplayName = pDTO.displayName, IsLobbyAdmin = pDTO.isLobbyAdmin, LobbyIndex=pDTO.lobbyIndex };
                playerList.Add(player);
            }

            joinLobbyArt.SetUpLobbyButton(lobby.id, playerList, lobbyList.IndexOf(lobby));
            
            newLobby.transform.SetParent(LobbyZone, false);

            lobbiesOnScreen.Add(newLobby);
        }
    }

    IEnumerator CriarLobby()
    {
        using(UnityWebRequest request = UnityWebRequest.Post(GlobalManager.URL + "/game", new WWWForm())){
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                LoadLobbies();
            }
        };
    }

    public void ConfirmNickname(){
        string name =  ModalPanel.GetComponentInChildren<HorizontalLayoutGroup>().GetComponentInChildren<TMP_InputField>().text;
        
        if(name != null && name != ""){
            name = name.ToUpper();
            GlobalManager.Instance.myPlayerName = name;
            NicknameText.GetComponent<TMP_Text>().text = name;
            ToggleModal(false);
            InitAfterNickname();
        }
    }

    public void UpdateOwnLobbyStatusOnCallback(Guid lobbyId, List<Player> players){
        GameObject lobby = lobbiesOnScreen.Find(l => new Guid(l.GetComponent<JoinLobbyArt>().Id) == lobbyId);
        lobby.GetComponent<JoinLobbyArt>().UpdatePlayersInLobby(players);
    }

    public void RemovePlayerFromLobby(Guid lobbyId, int playerIndex, bool myPlayerDisconnected){
        GameObject lobby = lobbiesOnScreen.Find(l => new Guid(l.GetComponent<JoinLobbyArt>().Id) == lobbyId);
        lobby.GetComponent<JoinLobbyArt>().RemovePlayerFromLobby(playerIndex, myPlayerDisconnected);
    }


    [Serializable]
    public class LobbyDTO{
        public String id;
        public Game[] games;
        public PlayerDTO[] players;
        public int team1Points;
        public int team2Points;
    }

    [Serializable]
    public class PlayerDTO{
        public String id;
        public String displayName;
        public Card[] cards;
        public bool isLobbyAdmin;
        public int lobbyIndex;
    }

    public class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
