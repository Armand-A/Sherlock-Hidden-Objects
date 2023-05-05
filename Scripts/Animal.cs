using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public bool found { get; set; }
    public bool near { get; set; }

    public GameObject spr;
    public GameObject anim;
    public GameObject bbox;

    // Start is called before the first frame update
    void Awake()
    {
        spr = this.transform.GetChild(0).gameObject;
        anim = this.transform.GetChild(1).gameObject;
        bbox = this.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collect()
    {
        found = true;
        spr.SetActive(false);
        anim.SetActive(true);
    }
    
    public void hide()
    {
        spr.SetActive(false);
        anim.SetActive(false);
    }
    public void show()
    {
        if (found){
            anim.SetActive(true);
        } else {
            spr.SetActive(true);
        }
    }
}
