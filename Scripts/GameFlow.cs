using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour 
{
    public GameObject bg_p;
    public GameObject bg_l; 
    public Animal[] animals_p;
    public Animal[] animals_l;
    public GameObject glass;
    
    public GameObject UI_p;
    public GameObject UI_l;
    public GameObject cta_p;
    public GameObject cta_l;

    public AudioSource sfxa; 
    public AudioSource sfxb;

    GameObject glass_bounding;
    GameObject sparkle;

    string savedOrient = "p";
    string currOrient = "p";

    bool cta_started;

    

    void Start(){
        glass_bounding = glass.transform.GetChild(0).gameObject;
        sparkle = glass.transform.GetChild(1).gameObject;
        switchOrient(calculateOrient());
    }

    void Update()
    {
        currOrient = calculateOrient();
        if (savedOrient != currOrient) {
            savedOrient = currOrient;
            switchOrient(currOrient);
        }

        int foundnum = 0;   
        for (int i=0; i<getAnimals().Length; i++){
            if (getAnimals()[i].found){
                foundnum++;
                getAnimals()[i].near = false; 
            }
            else {
                if (Vector3.Distance(getAnimals()[i].bbox.transform.position, glass_bounding.transform.position) < 1.5f) {
                    getAnimals()[i].near = true;
                    if (Vector3.Distance(getAnimals()[i].bbox.transform.position, glass_bounding.transform.position) < 1f) {
                        StartCoroutine(findDelay(i));
                    }
                }
                else {
                    getAnimals()[i].near = false; 
                }
            }
            
        }

        int nearcount = 0; 
        for (int i=0; i<getAnimals().Length; i++){
            if (getAnimals()[i].near == true){
                nearcount++;
            }
        }

        if (nearcount!=0){
            sparkle.SetActive(true);
        } else {
            sparkle.SetActive(false);
        }

        if (foundnum == 5 && !cta_started){
            cta_started = true;
            StartCoroutine(endingDelay());
        }

    }

    IEnumerator findDelay(int i)
    {
        yield return new WaitForSecondsRealtime(0.75f);
        if (Vector3.Distance(getAnimals()[i].bbox.transform.position, glass_bounding.transform.position) < 1f 
            && getAnimals()[i].found==false) 
        {
            getAnimals()[i].collect();
            sfxa.Play();
            sfxb.Play();
        }
        
    }

    IEnumerator endingDelay()
    {
        yield return new WaitForSecondsRealtime(1.75f);
        startCTA();
    }

    IEnumerator lndAdjustment()
    {
        for (float p=2, s=1; p >= 0.02; p -= 0.02f){ //100 
            yield return new WaitForSecondsRealtime(0.0001f);
            s += 0.003f;
            cta_l.transform.position = new Vector3(0, p, 0);
            cta_l.transform.localScale = new Vector3(s, s, 1);
        }
        
        cta_l.GetComponent<Animator>().enabled = true;
        cta_l.GetComponent<CTA>().enabled = true;
    }

    void startCTA()
    {
        UI_l.SetActive(false);
        UI_p.SetActive(false);
        glass.SetActive(false);

        if (currOrient == "p"){
            cta_p.GetComponent<Animator>().enabled = true;
            cta_p.GetComponent<CTA>().enabled = true;
        } else {
            /*cta_l.transform.position = new Vector3(0, 0, 0);
            cta_l.transform.localScale = new Vector3(1.3f, 1.3f, 1);
            cta_l.GetComponent<Animator>().enabled = true;
            cta_l.GetComponent<CTA>().enabled = true;*/
            StartCoroutine(lndAdjustment());

        }

        for (int i=0; i<getAnimals().Length; i++){
            getAnimals()[i].hide();
        }
    }

    void switchOrient(string o)
    {
        if (o == "p") {
            glass.transform.position = new Vector3(0, 2f, 0);
            bg_l.SetActive(false);
            bg_p.SetActive(true);

            cta_p.SetActive(true);

            UI_l.SetActive(false);
            cta_l.SetActive(false);
            if (!cta_started){
                UI_p.SetActive(true);
                for (int i=0; i<animals_p.Length; i++){
                    animals_p[i].show();
                    animals_l[i].hide();
                }
            } else {
                for (int i=0; i<animals_p.Length; i++){
                    animals_p[i].hide();
                    animals_l[i].hide();
                }
                cta_p.GetComponent<Animator>().enabled = true;
                cta_p.GetComponent<CTA>().enabled = true;
            }

        } else {
            glass.transform.position = new Vector3(0, 1.05f, 0);
            bg_l.SetActive(true);
            bg_p.SetActive(false);

            cta_l.SetActive(true);

            UI_p.SetActive(false);
            cta_p.SetActive(false);
            if (!cta_started){
                UI_l.SetActive(true);
                for (int i=0; i<animals_l.Length; i++){
                    animals_l[i].show();
                    animals_p[i].hide();
                }
            } else {
                for (int i=0; i<animals_l.Length; i++){
                    animals_l[i].hide();
                    animals_p[i].hide();
                }
                cta_l.transform.position = new Vector3(0, 0, 0);
                cta_l.transform.localScale = new Vector3(1.3f, 1.3f, 1);
                cta_l.GetComponent<Animator>().enabled = true;
                cta_l.GetComponent<CTA>().enabled = true;
            }
        }
    }

    Animal[] getAnimals()
    {
        if (currOrient == "p"){
            return animals_p;
        } else {
            return animals_l;
        }
    }

    String calculateOrient(){
        float screenRatio = (Screen.width/Screen.height);
        if (screenRatio >= 1) {
            return "l";
        } else {
            return "p";
        }
    }
}
