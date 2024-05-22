using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaTrigger : MonoBehaviour
{
    [SerializeField] private Puertas puerta;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Duolingo"))
        {
            puerta.doorOpen = true;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().GoToSpawnPoint();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Duolingo"))
        {
            puerta.doorOpen = false;
        }
    }
}
