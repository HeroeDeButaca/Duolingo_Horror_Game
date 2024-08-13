using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton_palabra_leccion : MonoBehaviour
{
    private DuoLesson duoLesson;
    public int id, orden;
    public string palabraBoton;
    private RectTransform rectTransform;
    public bool reordenar_palabras, esRespuesta, noCambiar;
    private float timeElapsed = 0, lerpDuration = 0.5f;
    public List<GameObject> palabrasIntroduccidas = new List<GameObject>();
    public Vector2 posicionInicial;
    void Start()
    {
        duoLesson = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (!noCambiar)
        {
            switch (orden)
            {
                case 1:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(-355, -10.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 2:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(-195, -10.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 3:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(-35, -10.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 4:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(125, -10.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 5:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(285, -10.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 6:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(445, -10.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 7:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(-355, -109.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 8:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(-195, -109.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
                case 9:
                    if (esRespuesta && timeElapsed < lerpDuration && !noCambiar)
                    {
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, new Vector3(-35, -109.5f, 0), timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else if (esRespuesta && timeElapsed >= lerpDuration && !noCambiar)
                    {
                        noCambiar = true;
                        timeElapsed = 0;
                    }
                    break;
            }
        }
        if (reordenar_palabras)
        {
            Reordenar_palabras();
        }
    }
    public void Click_boton()
    {
        if (!esRespuesta && !reordenar_palabras)
        {
            esRespuesta = true;
            duoLesson.palabrasIntroducidas++;
            orden = duoLesson.palabrasIntroducidas;
            duoLesson.palabrasEnUso.Add(this.gameObject);
            duoLesson.idIntroducido += id;
            duoLesson.fraseIntroducida += palabraBoton + " ";
        }
    }
    public void Reordenar_palabras()
    {
        if (timeElapsed < lerpDuration)
        {
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, posicionInicial, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else if (timeElapsed >= lerpDuration)
        {
            orden = 0;
            timeElapsed = 0;
            reordenar_palabras = false;
        }
    }
}
