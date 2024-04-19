using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linterna : MonoBehaviour
{
    [SerializeField]private Light luzLinterna;
    private bool activeLight;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activeLight = !activeLight;
        }
        if (activeLight)
        {
            luzLinterna.enabled = true;
        }
        else if (!activeLight)
        {
            luzLinterna.enabled = false;
        }
    }
}
