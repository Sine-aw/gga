using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTextDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform textTransform;
    public float downAmount = 5f;

    private Vector2 originalPos;

    void Start()
    {
        if (textTransform != null)
        {
            originalPos = textTransform.anchoredPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (textTransform != null)
        {
            textTransform.anchoredPosition = new Vector2(originalPos.x, originalPos.y - downAmount);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (textTransform != null)
        {
            textTransform.anchoredPosition = originalPos;
        }
    }
}