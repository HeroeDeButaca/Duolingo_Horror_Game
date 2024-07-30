using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puertas : MonoBehaviour
{
    public bool doorOpen, isInteractable = true;
    public float doorOpenAngle = 95, doorCloseAngle = 0, smooth = 3, time;
    Quaternion targetRotation, targetRotation2;
    private bool playSFX;
    private AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playSFX = false;
        time = 0;
    }
    public void ChangeDoorState()
    {
        if (isInteractable)
        {
            doorOpen = !doorOpen;
            playSFX = true;
        }
    }
    void Update()
    {
        if (doorOpen)
        {
            targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
            if (playSFX)
            {
                audioManager.PlaySFX(audioManager.abrirPuerta);
                playSFX = false;
            }
        }
        else
        {
            targetRotation2 = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
            if (playSFX && time < 1.27f)
            {
                time += Time.deltaTime;
            }
            else if (playSFX && time >= 1.27f)
            {
                audioManager.PlaySFX(audioManager.cerrarPuerta);
                playSFX = false;
                time = 0;
            }
        }
    }
}
