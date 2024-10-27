using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLessonPlace : MonoBehaviour
{
    [SerializeField] private GameObject[] ordenadores;
    private Material puertaIzqMat, puertaDerMat, puertaArrMat;
    private int activate;
    private float tiempo, tiempoCambio;
    private bool spaceStation;
    void Start()
    {
        CambioPantalla();
    }

    void Update()
    {
        if(tiempo < tiempoCambio)
        {
            tiempo += Time.deltaTime;
        }
        else if(tiempo >= tiempoCambio)
        {
            CambioPantalla();
            tiempoCambio = Random.Range(20, 40);
            tiempo = 0;
        }
    }
    private void CambioPantalla()
    {
        activate = Random.Range(0, (ordenadores.Length - 1));
        for (int i = 0; i < (ordenadores.Length - 1); i++)
        {
            if (i != activate)
            {
                ordenadores[i].gameObject.tag = "Untagged";
            }
            else if (i == activate)
            {
                ordenadores[i].gameObject.tag = "Ordenador_leccion";
            }
        }
    }
}
