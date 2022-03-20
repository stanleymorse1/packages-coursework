using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public GameObject prompt;
    private bool displayPrompt = false;
    public float lerpDuration = 0.3f;

    private float timeElapsed;
    private float startVal;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Debug.Log("Left click");
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right)
            Debug.Log("Right click");
    }

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
        if (displayPrompt)
        {
            prompt.GetComponent<CanvasGroup>().alpha = (Mathf.Lerp(startVal, 1, timeElapsed/lerpDuration));
        }
        else
        {
            prompt.GetComponent<CanvasGroup>().alpha = (Mathf.Lerp(startVal, 0, timeElapsed / lerpDuration));
        }
        timeElapsed += Time.deltaTime;
    }
}