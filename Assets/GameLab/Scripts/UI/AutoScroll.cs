using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private GameObject scrollContent;
    [SerializeField] private RectTransform contentRectTransform;
    [SerializeField] private ScrollRect scrollRect;
    double height;

    void Start(){
        height = contentRectTransform.rect.height;
    }

    public void ScrollToBottom(){
        Canvas.ForceUpdateCanvases();
        scrollRect.normalizedPosition = new Vector2(0, 0);
        height = contentRectTransform.rect.height;
    }
}
