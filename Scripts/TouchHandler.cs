using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class TouchHandler : MonoBehaviour 
{
    public GameObject helper;
    SpriteRenderer hand_spr; 

    Collider2D col;

    bool overlap;
    bool dragging;
    bool firstTouch;

    void Start()
    {
        col = GetComponent<Collider2D>();
        hand_spr = helper.GetComponent<SpriteRenderer>();
        StartCoroutine(helpingHand());
    }

    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            overlap = col == Physics2D.OverlapPoint(cursorPos) ? true : false;

            if (overlap)
            {
                if (!firstTouch){
                    helper.SetActive(false);
                    firstTouch = true;
                }
                dragging = true;
            }
        }

        if (dragging) {
            this.transform.position = new Vector2 (cursorPos.x - 0.26f, cursorPos.y + 0.26f);
            //this.transform.position = new Vector2 (cursorPos.x + 1f, cursorPos.y + 0.4f);
            //this.transform.position = new Vector2 (cursorPos.x - 0.73f, cursorPos.y - 0.12f);
        }

        if (Input.GetMouseButtonUp(0)) {
            overlap = false; 
            dragging = false;
        }
    }
    IEnumerator helpingHand()
    {
        while (!firstTouch){
            float alpha = 0;
            for (; alpha<1; alpha+=0.03f){
                yield return new WaitForSecondsRealtime(0.001f);
                hand_spr.color = new Color(1, 1, 1, alpha);
            }  
            yield return new WaitForSecondsRealtime(0.4f);    
            for (; alpha>=0; alpha-=0.03f){
                yield return new WaitForSecondsRealtime(0.001f);
                hand_spr.color = new Color(1, 1, 1, alpha);
            }
        }   
    }

}
