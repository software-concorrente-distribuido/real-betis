using System.Collections.Generic;
using UnityEngine;
using TrucoOnline.Models;
using System;
using TMPro;

public class Deck : MonoBehaviour
{
    public static Deck Instance;

    [System.Serializable]
    public class DeckCard
    {
        public string Value;
        public string Suit;
        public int Strength;
        public Sprite CardSprite;

        public DeckCard(string _value, string _suit, int _strength, Sprite sprite)
        {
            Value = _value;
            Suit = _suit;
            Strength = _strength;
            CardSprite = sprite;
        }
    }

    public static List<DeckCard> cardDeck = new List<DeckCard>();

    public GameObject cardPrefab;
    [SerializeField] Transform myHand;
    [SerializeField] Transform opponent1Hand;
    [SerializeField] Transform teammateHand;
    [SerializeField] Transform opponent2Hand;

    Dictionary<Guid, Transform> IdTransformMap = new Dictionary<Guid, Transform>();

    void Awake()
    {
        Instance = this;

        cardDeck = new List<DeckCard>();
        InstantiateDeck();
    }

    public void Init()
    {
        cardDeck = new List<DeckCard>();
        InstantiateDeck();
    }

    void InstantiateDeck()
    {
        cardDeck.Add(new DeckCard("4", "Clubs", 10, Resources.Load<Sprite>("4.7")));
        cardDeck.Add(new DeckCard("7", "Hearts", 9, Resources.Load<Sprite>("7.2")));
        cardDeck.Add(new DeckCard("A", "Spades", 8, Resources.Load<Sprite>("A.5")));
        cardDeck.Add(new DeckCard("7", "Diamonds", 7, Resources.Load<Sprite>("7.4")));

        cardDeck.Add(new DeckCard("3", "Clubs", 6, Resources.Load<Sprite>("3.7")));
        cardDeck.Add(new DeckCard("3", "Hearts", 6, Resources.Load<Sprite>("3.2")));
        cardDeck.Add(new DeckCard("3", "Spades", 6, Resources.Load<Sprite>("3.5")));
        cardDeck.Add(new DeckCard("3", "Diamonds", 6, Resources.Load<Sprite>("3.4")));
        
        cardDeck.Add(new DeckCard("2", "Clubs", 5, Resources.Load<Sprite>("2.7")));
        cardDeck.Add(new DeckCard("2", "Hearts", 5, Resources.Load<Sprite>("2.2")));
        cardDeck.Add(new DeckCard("2", "Spades", 5, Resources.Load<Sprite>("2.5")));
        cardDeck.Add(new DeckCard("2", "Diamonds", 5, Resources.Load<Sprite>("2.4")));
        
        cardDeck.Add(new DeckCard("A", "Clubs", 4, Resources.Load<Sprite>("A.7")));
        cardDeck.Add(new DeckCard("A", "Hearts", 4, Resources.Load<Sprite>("A.2")));
        cardDeck.Add(new DeckCard("A", "Diamonds", 4, Resources.Load<Sprite>("A.4")));
        
        cardDeck.Add(new DeckCard("K", "Clubs", 3, Resources.Load<Sprite>("K7")));
        cardDeck.Add(new DeckCard("K", "Hearts", 3, Resources.Load<Sprite>("K2")));
        cardDeck.Add(new DeckCard("K", "Spades", 3, Resources.Load<Sprite>("K5")));
        cardDeck.Add(new DeckCard("K", "Diamonds", 3, Resources.Load<Sprite>("K4")));
        
        cardDeck.Add(new DeckCard("Q", "Clubs", 2, Resources.Load<Sprite>("Q7")));
        cardDeck.Add(new DeckCard("Q", "Hearts", 2, Resources.Load<Sprite>("Q2")));
        cardDeck.Add(new DeckCard("Q", "Spades", 2, Resources.Load<Sprite>("Q5")));
        cardDeck.Add(new DeckCard("Q", "Diamonds", 2, Resources.Load<Sprite>("Q4")));
        
        cardDeck.Add(new DeckCard("J", "Clubs", 1, Resources.Load<Sprite>("J7")));
        cardDeck.Add(new DeckCard("J", "Hearts", 1, Resources.Load<Sprite>("J2")));
        cardDeck.Add(new DeckCard("J", "Spades", 1, Resources.Load<Sprite>("J5")));
        cardDeck.Add(new DeckCard("J", "Diamonds", 1, Resources.Load<Sprite>("J4")));
    }

    public CardObject HandOutOtherPlayersCards(Guid Id)
    {
        GameObject newCard = Instantiate(cardPrefab);
        CardObject nCard = newCard.GetComponent<CardObject>();
        nCard.SetUpCard("", "", 0, null, false);
        newCard.transform.SetParent(IdTransformMap[Id], false);

        return nCard;
    }

    public CardObject HandOutPlayerCard(Card cardToInstantiate){
        GameObject newCard = Instantiate(cardPrefab);
        DeckCard temp = CardObject.GetCardObjectFromServerCard(cardToInstantiate);
        CardObject nCard = newCard.GetComponent<CardObject>();
        nCard.SetUpCard(temp.Suit, temp.Value, temp.Strength, temp.CardSprite, true);
        newCard.transform.SetParent(myHand, false);

        return nCard;
    }

    //FUNCAO PRA DEFERIR O ID DE CADA JOGADOR A UM TRANSFORM DO JOGO
    public void DeferPlayerIdToTransform(List<Player> players, Guid myId){
        IdTransformMap.Add(myId, myHand);

        int myIndex = players.FindIndex(p => p.Id.Equals(myId));
        bool evenIndex = myIndex % 2 == 0;
        int idx = 0;
        bool opp1 = false;

        foreach(Player player in players){
            int team = (idx%2) + 1;

            if(!player.Id.Equals(myId)){
                if(evenIndex == (idx % 2 == 0)){
                    IdTransformMap.Add(player.Id, teammateHand);
                    GameObject.Find("TeamNick").GetComponent<TMP_Text>().text = player.DisplayName;    
                    GameObject.Find("TeammateTeam").GetComponent<TMP_Text>().text = "Time " + team;    
                } 
                else{
                    if(myIndex != 3 && myIndex != 0){
                        if(idx < myIndex){
                            IdTransformMap.Add(player.Id, opponent2Hand);
                            GameObject.Find("Opp2Nick").GetComponent<TMP_Text>().text = player.DisplayName; 
                            GameObject.Find("Opp2Team").GetComponent<TMP_Text>().text = "Time " + team;
                        } 
                        else{
                            IdTransformMap.Add(player.Id, opponent1Hand);
                            GameObject.Find("Opp1Nick").GetComponent<TMP_Text>().text = player.DisplayName; 
                            GameObject.Find("Opp1Team").GetComponent<TMP_Text>().text = "Time " + team; 
                        } 
                    }
                    else{
                        if(!opp1){
                            IdTransformMap.Add(player.Id, opponent1Hand);
                            GameObject.Find("Opp1Nick").GetComponent<TMP_Text>().text = player.DisplayName; 
                            GameObject.Find("Opp1Team").GetComponent<TMP_Text>().text = "Time " + team;   
                            opp1 = true;
                        }
                        else{
                            IdTransformMap.Add(player.Id, opponent2Hand);
                            GameObject.Find("Opp2Nick").GetComponent<TMP_Text>().text = player.DisplayName;   
                            GameObject.Find("Opp2Team").GetComponent<TMP_Text>().text = "Time " + team;  
                        } 
                    }
                }
            }
            else{
                GameObject.Find("MyNick").GetComponent<TMP_Text>().text = player.DisplayName;   
                GameObject.Find("MyTeam").GetComponent<TMP_Text>().text = "Time " + team;
            }
        
            idx++;
        }
    }

    public void HandleIsMyTurn(bool isMyTurn){
        GameManager.Instance.YourTurn.SetActive(isMyTurn);
        ToggleMyHand(isMyTurn);

        if(!isMyTurn){
            GameManager.Instance.TrucoButton.SetActive(false);
        }
        else{
            if(GlobalManager.Instance.trucosCalled != 4){
                if(!GameManager.Instance.TrucoRequested){
                    GameManager.Instance.TrucoButton.SetActive(true);
                }
                else if(GameManager.Instance.TrucoRequested && GameManager.Instance.TeamRequestTruco == GlobalManager.Instance.myPlayerIndex % 2){
                    GameManager.Instance.TrucoButton.SetActive(false);
                }
                else {
                    GameManager.Instance.TrucoButton.SetActive(true);
                }
            }
            else GameManager.Instance.TrucoButton.SetActive(false);
        }
    }

    public void ToggleMyHand(bool isMyTurn){
        for(int i = 0; i < myHand.childCount; i++){
            myHand.GetChild(i).GetComponent<Drag>().enabled = isMyTurn;
        }
    }

    public void RemoveRemainingCards(Guid Id){
        Transform t = IdTransformMap[Id];
        while(t.childCount > 0) DestroyImmediate(t.GetChild(0).gameObject);
    }

    public void RemoveCardTransformFromPlayerHand(Guid playerId){
        Transform transform = IdTransformMap[playerId];
        
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }

    public void InstantiatePlayedCardOnDropZone(Card playedCard){
        Transform dropZoneTransform = GameObject.Find("DropZone").transform;

        GameObject newCard = Instantiate(cardPrefab);
        DeckCard temp = CardObject.GetCardObjectFromServerCard(playedCard);
        CardObject nCard = newCard.GetComponent<CardObject>();
        nCard.SetUpCard(temp.Suit, temp.Value, temp.Strength, temp.CardSprite, true);
        newCard.transform.SetParent(dropZoneTransform, false);

        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.AudioClip);
    }

}
