using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickNotification : MonoBehaviour
{
    private RectTransform notiTransform;
    [SerializeField] private float t1, t2, finalX, initialY;
    private bool bajarTime = false, stop = false;
    private Button button;
    private DuoLesson duoLesson;
    private Notificaciones notificaciones;
    void Start()
    {
        duoLesson = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>();
        notiTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(activateNotification);
        initialY = Random.Range(-260, 239);
        finalX = Random.Range(-492, 560);
        notiTransform.localPosition = new Vector2(1350, initialY);
        notificaciones = GameObject.FindGameObjectWithTag("Notificaciones").GetComponent<Notificaciones>();
    }

    void Update()
    {
        //notiTransform.position = new Vector3(Mathf.Lerp(notiTransform.position.x, finalX, t1), notiTransform.position.y, 0);
        //notiTransform.localPosition = Vector3.Lerp(notiTransform.localPosition, new Vector2(finalX, notiTransform.localPosition.y), t1);
        if(t1 < 1 && !bajarTime && !stop)
        {
            t1 += Time.deltaTime * 0.4f;
            notiTransform.localPosition = Vector3.Lerp(notiTransform.localPosition, new Vector2(finalX, notiTransform.localPosition.y), t1);
        }
        else if(t1 >= 1 && !bajarTime && !stop)
        {
            t2 += Time.deltaTime;
            if(t2 >= 2.5f)
            {
                t1 = 0;
                bajarTime = true;
            }
        }

        if(t1 < 1 && bajarTime && !stop)
        {
            t1 += Time.deltaTime * 0.2f;
            button.interactable = false;
            notiTransform.localPosition = Vector3.Lerp(notiTransform.localPosition, new Vector2(1350, notiTransform.localPosition.y), t1);
        }
        else if(t1 >= 1 && bajarTime && !stop)
        {
            notificaciones.resetParameters();
            Destroy(gameObject);
        }
    }
    public void activateNotification()
    {
        stop = true;
        t1 = 1.1f;
        t2 = 2.6f;
        notificaciones.resetParameters();
        duoLesson.activateNotificacion();
        Destroy(gameObject);
    }
}
