using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta_Space : MonoBehaviour
{
    [SerializeField] private Transform puertaTransform;
    private Vector3 closedPosition;
    [SerializeField] private Vector3 openPosition;
    [SerializeField] private float t1 = 0;
    private bool abierto = false;
    [SerializeField] private bool playerCantOpen;
    void Start()
    {
        closedPosition = puertaTransform.position;
    }

    void Update()
    {
        if (!abierto && puertaTransform.position != closedPosition)
        {
            if(t1 < 1)
            {
                t1 += Time.deltaTime;
            }
            puertaTransform.position = Vector3.Slerp(puertaTransform.position, closedPosition, t1);
        }
        else if(abierto && puertaTransform.position != openPosition)
        {
            if(t1 < 1)
            {
                t1 += Time.deltaTime;
            }
            puertaTransform.position = Vector3.Slerp(puertaTransform.position, openPosition, t1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !playerCantOpen || other.gameObject.CompareTag("Duolingo"))
        {
            abierto = true;
            t1 = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerCantOpen || other.gameObject.CompareTag("Duolingo"))
        {
            abierto = false;
            t1 = 0;
        }
    }
}
