using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLessonPC : MonoBehaviour
{
    [SerializeField] private GameObject[] pantallas;
    private int pantallaEncendida;
    private float transcurredTime, timeToChange;
    private bool gameStart;
    void Start()
    {
        timeToChange = Random.Range(35, 50);
        pantallaEncendida = Random.Range(0, pantallas.Length - 1);
        for(int i = 0; i < (pantallas.Length - 1); i++)
        {
            if(i != pantallaEncendida)
            {
                pantallas[i].tag = "Untagged";
            }
            else if(i == pantallaEncendida)
            {
                pantallas[i].tag = "Ordenador_leccion";
            }
        }
    }
    void Update()
    {
        if (gameStart && transcurredTime < timeToChange)
        {
            transcurredTime += Time.deltaTime;
        }
        else if(gameStart && transcurredTime >= timeToChange)
        {

        }
    }
}
