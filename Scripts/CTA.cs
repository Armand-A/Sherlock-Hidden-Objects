using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class CTA : MonoBehaviour 
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Luna.Unity.Playable.InstallFullGame();
        }
    }

}
