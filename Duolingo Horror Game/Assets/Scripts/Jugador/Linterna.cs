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
    private float lanAtaCooldown = 0, t;
    public bool canShoot = true, lanzaAtaque = false;
    [SerializeField] private GameObject textoNoBateria, textoNoBateria2;
    [SerializeField] private Transform centroAtaque;
    private AudioManager audioManager;
    [SerializeField] private float distancia = 15;
    [SerializeField] private Image ataqueLinternaUI;
    private DuoController duoController;

    // Phantom Duo jumpscare
    [SerializeField] private CanvasGroup canvasPDjumpscare;
    [SerializeField] private Transform imageJumpscare;
    private PlayerMovement playerMovement;
    private float tImageJumpscare;
    private bool pdJumpscare = false, audioJump = false;
    [SerializeField] private GlobalVolumeScript volumeScript;
    private PhantomDuoController phantomDuo;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        duoController = GameObject.FindGameObjectWithTag("Duolingo").GetComponent<DuoController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        phantomDuo = GameObject.FindGameObjectWithTag("PhantomDuo").GetComponent<PhantomDuoController>();
        noche3 = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameOver>().nocheSeleccionada == 2 ? true : false;
        if (!noche3)
        {
            cargas = 3;
            maxCargas = 3;
        }
        else if (noche3)
        {
            cargas = 5;
            maxCargas = 5;
            ataqueLinternaUI = GameObject.FindGameObjectWithTag("SuperAtaque").GetComponent<Image>();
        }
    }

    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow, distancia);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia))
        {
            if(hit.collider.CompareTag("Duolingo") && lanzaAtaque)
            {
                duoController.oneTimeBool = true;
                duoController.duoDissapear = true;
                duoController.duoNavMesh.enabled = false;
            }
            if(hit.collider.CompareTag("PhantomDuo") && activeLight)
            {
                playerMovement.quieto = true;
                playerMovement.movimientoActivo = false;
                phantomDuo.oneTimeBool = true;
                activeLight = false;
                canvasPDjumpscare.alpha = 1;
                tImageJumpscare = 0;
                audioJump = false;
                volumeScript.cambiarVignette(true);
                phantomDuo.oneTimeBool = true;
                phantomDuo.pdJumpscare = true;
            }
        }
        if (lanzaAtaque)
        {
            if(lanAtaCooldown < 0.1f)
            {
                lanAtaCooldown += Time.deltaTime;
            }
            else if(lanAtaCooldown >= 0.1f)
            {
                lanzaAtaque = false;
            }
        }
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
            if(luzLinterna.intensity > 10)
            {
                bajarIntensity = true;
            }
        }
        if(cargas == 0 && textoNoBateria.activeSelf)
        {
            textoNoBateria.GetComponent<TMP_Text>().color = new Vector4(1, 1, 1, Mathf.Lerp(1, 0, t));
            if (sube)
            {
                t -= 0.6f * Time.deltaTime;
            }
            else if (!sube)
            {
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
            if (luzLinterna.intensity < 22)
            {
                luzLinterna.intensity += Time.deltaTime * 14;
            }
        }
        if(canShoot & luzLinterna.intensity >= 22 & Input.GetMouseButtonUp(0) & activeLight & cargas > 0)
        {
            audioManager.PlaySFX(audioManager.ataqueLanzado);
            lanAtaCooldown = 0;
            bajarIntensity = true;
            lanzaAtaque = true;
            cargas -= 1;
            CargasBateria();
            
        }
        else if(luzLinterna.intensity < 22 & Input.GetMouseButtonUp(0) & activeLight & cargas > 0)
        {
            bajarIntensity = true;
        }

        if (luzLinterna.intensity > 10 && bajarIntensity)
        {
            luzLinterna.intensity -= Time.deltaTime * 35;
        }
        else if (luzLinterna.intensity <= 10 && bajarIntensity)
        {
            luzLinterna.intensity = 10;
            bajarIntensity = false;
        }
        
        if(luzLinterna.intensity <= 10 && ataqueLinternaUI.fillAmount == 0)
        {
            ataqueLinternaUI.fillAmount = 0;
        }
        else if(luzLinterna.intensity >= 22 && ataqueLinternaUI.fillAmount == 1)
        {
            ataqueLinternaUI.fillAmount = 1;
        }
        else
        {
            ataqueLinternaUI.fillAmount = (luzLinterna.intensity - 10) / 12;
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
                    imageCargas[3].color = new Vector4(1, 1, 1, 0);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0);
                    imageCargas[5].color = new Vector4(1, 1, 1, 0);
                    imageCargas[6].color = new Vector4(1, 1, 1, 0);
                    imageCargas[7].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria2.SetActive(true);
                    break;
                case 1:
                    imageCargas[3].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0);
                    imageCargas[5].color = new Vector4(1, 1, 1, 0);
                    imageCargas[6].color = new Vector4(1, 1, 1, 0);
                    imageCargas[7].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria2.SetActive(false);
                    break;
                case 2:
                    imageCargas[3].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[5].color = new Vector4(1, 1, 1, 0);
                    imageCargas[6].color = new Vector4(1, 1, 1, 0);
                    imageCargas[7].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria2.SetActive(false);
                    break;
                case 3:
                    imageCargas[3].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[5].color = new Vector4(1, 1, 1, 0.6f);
                    imageCargas[6].color = new Vector4(1, 1, 1, 0);
                    imageCargas[7].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria2.SetActive(false);
                    break;
                case 4:
                    imageCargas[3].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[5].color = new Vector4(1, 1, 1, 0.6f);
                    imageCargas[6].color = new Vector4(1, 1, 1, 0.8f);
                    imageCargas[7].color = new Vector4(1, 1, 1, 0);
                    textoNoBateria2.SetActive(false);
                    break;
                case 5:
                    imageCargas[3].color = new Vector4(1, 1, 1, 0.2f);
                    imageCargas[4].color = new Vector4(1, 1, 1, 0.4f);
                    imageCargas[5].color = new Vector4(1, 1, 1, 0.6f);
                    imageCargas[6].color = new Vector4(1, 1, 1, 0.8f);
                    imageCargas[7].color = new Vector4(1, 1, 1, 1);
                    textoNoBateria2.SetActive(false);
                    break;
            }
        }
    }
}
