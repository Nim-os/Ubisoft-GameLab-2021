using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollIssue : MonoBehaviour
{
    GameObject content;
    double height;

    void Start(){
        //record the height of the current rect
        content=this.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        height=content.GetComponent<RectTransform>().rect.height;
    }


    // Update is called once per frame
    void Update()
    {
        //update the position of the scroll rect once the height change
        if(content.GetComponent<RectTransform>().rect.height>height){
            this.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
            height=content.GetComponent<RectTransform>().rect.height;
        }
        
    }
}
