using TrucoOnline.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    GlobalManager GlobalManager;

    void Awake(){
        GlobalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null){
            eventData.pointerDrag.GetComponent<Transform>().rotation = Quaternion.Euler(0,0,0);
            eventData.pointerDrag.GetComponent<CardObject>().TurnCard();
            eventData.pointerDrag.GetComponent<Transform>().SetParent(this.transform);

            //GameManager.Instance.PlayCard(eventData.pointerDrag.GetComponent<CardObject>());

            CardObject cardObject = eventData.pointerDrag.GetComponent<CardObject>();
            GameManager.Instance.RemoveCardFromHand(cardObject);

            GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.AudioClip);

            Card card = Card.CardObjectToCard(cardObject);
            GlobalManager.PlayCard(card);
        }

        
    }
    
}
