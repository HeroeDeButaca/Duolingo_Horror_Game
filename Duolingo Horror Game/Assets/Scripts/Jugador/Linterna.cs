using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Linterna : MonoBehaviour
{
    [SerializeField] private Light luzLinterna;
    [SerializeField] private RawImage[] imageCargas;
    public int cargas, maxCargas;
    private bool activeLight, noche3, bajarIntensity = false, sube = false;
    private float cargandoAtaque, t;
    public bool canShoot = true, lanzaAtaque = false;
    [SerializeField] private GameObject textoNoBateria, prefabDisparo;
    [SerializeField] private Transform centroAtaque;
    void Start()
    {
        noche3 = PlayerPrefs.GetInt("Noche3") == 1 ? true : false;
        if (!noche3)
        {
            cargas = 3;
            maxCargas = 3;
        }
        else if (noche3)
        {
            cargas = 5;
            maxCargas = 5;
        }
        
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
        if(cargas == 0 && textoNoBateria.activeSelf)
        {
            textoNoBateria.GetComponent<TMP_Text>().color = new Vector4(1, 1, 1, Mathf.Lerp(1, 0, t));
            if (sube)
            {
                Debug.Log("Sube alpha");
                t -= 0.6f * Time.deltaTime;
            }
            else if (!sube)
            {
                Debug.Log("Baja alpha");
                t += 0.6f * Time.deltaTime;
            }
            if(textoNoBateria.GetComponent<TMP_Text>().color.a == 1)
            {
                sube = false;
            }
            else if(textoNoBateria.GetComponent<TMP_Text>().color.a == 0)
            {
                sube = true;
            }
        }
        // Ataque linterna
        if(canShoot & Input.GetMouseButton(0) & activeLight & cargas > 0)
        {
            Debug.Log("Cargando ataque linterna...");
            cargandoAtaque += Time.deltaTime;
            if (luzLinterna.intensity < 22)
            {
                luzLinterna.intensity += Time.deltaTime * 8;
            }
        }
        if(canShoot & cargandoAtaque >= 1.5f & Input.GetMouseButtonUp(0) & activeLight & cargas > 0)
        {
            Debug.Log("¡Ataque!");
            cargandoAtaque = 0;
            bajarIntensity = true;
            lanzaAtaque = true;
            Instantiate(prefabDisparo, centroAtaque.position, centroAtaque.rotation);
            cargas -= 1;
            CargasBateria();
            
        }
        else if(cargandoAtaque < 1.5f & Input.GetMouseButtonUp(0) & activeLight & cargas > 0)
        {
            Debug.Log("Ataque no cargado");
            cargandoAtaque = 0;
            bajarIntensity = true;
        }

        if (luzLinterna.intensity < 10 && bajarIntensity)
        {
            luzLinterna.intensity -= Time.deltaTime * 8;
        }
        else if (luzLinterna.intensity >= 10 && bajarIntensity)
        {
            luzLinterna.intensity = 10;
            bajarIntensity = false;
        }
    }
    public void CargasBateria()
    {
        if(maxCargas == 3)
        {
            switch (cargas)
            {
                case 0:
                    imageCargas[0].color = new Vector4(0, 0, 0, 0);
                    imageCargas[1].color = new Vector4(0, 0, 0, 0);
                    imageCargas[2].color = new Vector4(0, 0, 0, 0);
                    textoNoBateria.SetActive(true);
                    break;
                case 1:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.33f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria.SetActive(false);
                    break;
                case 2:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.33f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0.66f);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria.SetActive(false);
                    break;
                case 3:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.33f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0.66f);
                    imageCargas[2].color = new Vector4(1, 1, 1, 1);
                    textoNoBateria.SetActive(false);
                    break;
            }
        }
        else if(maxCargas == 5)
        {
            switch (cargas)
            {
                case 0:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0);
                    imageCargas[3].color = new Vector4(1, 1, 1, 0);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria.SetActive(true);
                    break;
                case 1:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0);
                    imageCargas[3].color = new Vector4(1, 1, 1, 0);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria.SetActive(false);
                    break;
                case 2:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0);
                    imageCargas[3].color = new Vector4(1, 1, 1, 0);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria.SetActive(false);
                    break;
                case 3:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0.6f);
                    imageCargas[3].color = new Vector4(1, 1, 1, 0);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria.SetActive(false);
                    break;
                case 4:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0.6f);
                    imageCargas[3].color = new Vector4(1, 1, 1, 0.8f);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria.SetActive(false);
                    break;
                case 5:
                    imageCargas[0].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[1].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[2].color = new Vector4(1, 1, 1, 0.6f);
                    imageCargas[3].color = new Vector4(1, 1, 1, 0.8f);
                    imageCargas[4].color = new Vector4(1, 1, 1, 1);
                    textoNoBateria.SetActive(false);
                    break;
            }
        }
    }
}
