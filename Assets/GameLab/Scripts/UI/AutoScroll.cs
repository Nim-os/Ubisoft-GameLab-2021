using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    //[SerializeField]
    //public GameObject content;
    //RectTransform rectObj;

    void Start(){
        //record the height of the current rect
        //rectObj = content.GetComponent<RectTransform>();
        
    }

    public void ScrolltoBottom()
    {
        this.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);//scroll to the bottom of the rect
        
    }
}
