using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 relativeStartPosition;
    Transform originalParent = null;
    Transform originalParentParent = null;
    CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        relativeStartPosition = this.transform.position - Input.mousePosition;
        this.transform.SetParent(this.transform.parent.parent);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position + relativeStartPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(this.transform.parent == originalParentParent){
            this.transform.SetParent(originalParent);
        }
        canvasGroup.blocksRaycasts = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        originalParent = this.transform.parent;
        originalParentParent = this.transform.parent.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
