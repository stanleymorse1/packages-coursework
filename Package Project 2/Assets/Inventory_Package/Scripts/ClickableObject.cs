using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public GameObject prompt;
    public GameObject item;
    private bool displayPrompt = false;
    public float lerpDuration = 0.3f;

    private float timeElapsed;
    private float startVal;

    private Item itemScript;
    private CharInventory inventory;

    private void Start()
    {
        itemScript = item.GetComponent<Item>();
        inventory = GameObject.FindWithTag("Player").GetComponent<CharInventory>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && itemScript.usable)
        {
            Debug.Log($"Used {itemScript.itemName}");
            itemScript.use.Invoke();
        }
        //else if (eventData.button == PointerEventData.InputButton.Middle)
        //    Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right && itemScript.droppable)
        {
            Debug.Log($"Dropped {itemScript.itemName}");
            inventory.SendMessage("dropItem", item);
        }
    }

    // Detect when player is hovering over this item in the inventory
    public void onPointerEnter()
    {
        startVal = prompt.GetComponent<CanvasGroup>().alpha;
        displayPrompt = true;
        timeElapsed = 0;
    }
    public void onPointerExit()
    {
        startVal = prompt.GetComponent<CanvasGroup>().alpha;
        displayPrompt = false;
        timeElapsed = 0;
    }
    private void Update()
    {
        float currentVal = prompt.GetComponent<CanvasRenderer>().GetAlpha();
        if (displayPrompt && (itemScript.usable || itemScript.droppable))
        {
            prompt.GetComponent<CanvasGroup>().alpha = (Mathf.Lerp(startVal, 1, timeElapsed/lerpDuration));
        }
        else
        {
            prompt.GetComponent<CanvasGroup>().alpha = (Mathf.Lerp(startVal, 0, timeElapsed / lerpDuration));
        }
        if(item.GetComponent<Item>().image != null)
        {
            prompt.transform.parent.Find("Name").GetComponent<CanvasGroup>().alpha = currentVal;
        }
        timeElapsed += Time.deltaTime;
    }
}