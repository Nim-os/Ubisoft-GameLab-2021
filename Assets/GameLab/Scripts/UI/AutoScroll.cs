using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField]
    private GameObject scrollContent;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private ScrollRect scrollRect;
    double height;

    void Start(){
        height = rectTransform.rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        //update the position of the scroll rect once the height change
        if(rectTransform.rect.height > height){
            scrollRect.normalizedPosition = new Vector2(0, 0);
            height = rectTransform.rect.height;
        }  
    }
}
