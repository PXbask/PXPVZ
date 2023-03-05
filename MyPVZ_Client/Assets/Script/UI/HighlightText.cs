using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightText : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    Text text;
    Color originColor;
    public Color highlightColor;
    public Color clickedColor;
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originColor;
    }

    private void Awake()
    {
        text= GetComponent<Text>();
        originColor = text.color;
    }
}
