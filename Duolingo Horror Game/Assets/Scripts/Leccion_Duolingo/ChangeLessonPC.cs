using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLessonPC : MonoBehaviour
{
    [SerializeField] private GameObject[] pantallas, negro;
    private int pantallaEncendida;
    [SerializeField] private float transcurredTime, timeToChange;
    public bool gameStart;
    void Start()
    {
        timeToChange = Random.Range(35, 50);
        pantallaEncendida = Random.Range(0, (pantallas.Length));
        if(pantallaEncendida == pantallas.Length)
        {
            pantallaEncendida = 2;
        }
        Debug.Log((pantallas.Length - 1));
        for(int i = 0; i < (pantallas.Length); i++)
        {
            if (i != pantallaEncendida)
            {
                pantallas[i].tag = "Untagged";
                negro[i].SetActive(true);
            }
            else if(i == pantallaEncendida)
            {
                pantallas[i].tag = "Ordenador_leccion";
                if (negro[i])
                {
                    negro[i].SetActive(false);
                }
                
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
            pantallaEncendida = Random.Range(0, (pantallas.Length));
            if (pantallaEncendida == pantallas.Length)
            {
                pantallaEncendida = 2;
            }
            for(int i = 0; i < (pantallas.Length); i++)
            {
                if(pantallaEncendida == i)
                {
                    pantallas[i].tag = "Ordenador_leccion";
                    negro[i].SetActive(false);
                }
                else if(pantallaEncendida != i)
                {
                    pantallas[i].tag = "Untagged";
                    negro[i].SetActive(true);
                }
            }
            timeToChange = Random.Range(35, 50);
            transcurredTime = 0;
        }
    }
}
