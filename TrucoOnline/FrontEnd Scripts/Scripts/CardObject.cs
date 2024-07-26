using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrucoOnline.Models;
using UnityEngine;
using UnityEngine.UI;
using static Deck;

public class CardObject : MonoBehaviour
{
    [SerializeField] Sprite cardBackSprite;
    [SerializeField] Image cardImage;

    public string Suit {get; set;}
    public string Value {get; set;}
    public int Strength {get; set;}
    public Sprite CardSprite;
    public bool IsFacedUpwards;
    public int HolderIndex;

    public void SetUpCard(string suit, string value, int strength, Sprite cardSprite, bool isPlayer){
        Suit = suit;
        Value = value;
        Strength = strength;
        CardSprite = cardSprite;

        if(!isPlayer){
            IsFacedUpwards = false;
            cardImage.sprite = cardBackSprite;
        }
        else {
            IsFacedUpwards = true;
            cardImage.sprite = CardSprite;
        }
    }

    public void TurnCard(){
        if(!IsFacedUpwards){
            cardImage.sprite = CardSprite;
        }
    }

    public static DeckCard GetCardObjectFromServerCard(Card serverCard)
    {
        var cardDeck = Deck.cardDeck;
        return cardDeck.First(c => c.Value == serverCard.Value && c.Suit == serverCard.Suit.ToString());
    }
}
