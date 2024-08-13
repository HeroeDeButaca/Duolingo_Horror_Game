using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notificaciones : MonoBehaviour
{
    [SerializeField] private float timeToAppear, t1, initialY;
    [SerializeField] private GameObject notificationPrefab;
    public bool gameStart;
    private bool notificationInScreen, instantiate, oneTime;
    [SerializeField] private Transform parent;
    //private DuoLesson duoLesson;
    void Start()
    {
        //duoLesson = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>();
        timeToAppear = Random.Range(15, 25);
        notificationInScreen = false;
    }

    void Update()
    {
        if (gameStart)
        {
            NotificationAppear();
        }

        if (instantiate)
        {
            instantiatePrefab();
        }
    }
    private void NotificationAppear()
    {
        if(t1 < timeToAppear && !notificationInScreen)
        {
            t1 += Time.deltaTime;
        }
        else if(t1 >= timeToAppear && !notificationInScreen)
        {
            instantiate = true;
            notificationInScreen = true;
            t1 = 0;
            oneTime = true;
            timeToAppear = Random.Range(15, 25);
        }
    }
    private void instantiatePrefab()
    {
        if (oneTime)
        {
            Instantiate(notificationPrefab, new Vector2(1350, 0), Quaternion.identity, parent);
            instantiate = false;
            oneTime = false;
        }
    }
    public void resetParameters()
    {
        timeToAppear = Random.Range(15, 25);
        t1 = 0;
        notificationInScreen = false;
    }
}
