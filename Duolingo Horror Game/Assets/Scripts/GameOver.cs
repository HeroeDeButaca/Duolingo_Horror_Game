using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] canvasGroups;
    private bool quitarCanvasNegro = true, ponerCanvasNegro = false, t2Aumenta = true, moverCamara = false, oneTimeBool = false, oneTimeBool2 = false;
    public bool partyEnd = false;
    private int newScene;
    public int nocheSeleccionada = 1;
    private float t = 0, tES, t2 = 0, t3, susto = 0, tImageJumpscare = 0;
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite[] sprite;
    [SerializeField] private Transform camaraTransf, imageJumpscare;
    [SerializeField] private TMP_Text textoPuntos, numNocheActual, segundos;
    private PlayerMovement playerMovement;
    private DuoController duoController;
    [SerializeField] private GameObject linterna;
    [SerializeField] private VideoPlayer videoPlayer;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        duoController = GameObject.FindGameObjectWithTag("Duolingo").GetComponent<DuoController>();
        images[1].sprite = sprite[0];
        nocheSeleccionada = 1;
    }
    void Update()
    {
        if (canvasGroups[0].alpha != 0 && quitarCanvasNegro)
        {
            canvasGroups[0].alpha = Mathf.Lerp(1, 0, t);
            t += 1 * Time.deltaTime;
        }
        else if(canvasGroups[0].alpha == 0 && quitarCanvasNegro)
        {
            quitarCanvasNegro = false;
            canvasGroups[0].blocksRaycasts = false;
            t = 0;
        }

        if (canvasGroups[0].alpha != 1 && ponerCanvasNegro)
        {
            canvasGroups[0].alpha = Mathf.Lerp(0, 1, t);
            t += 1 * Time.deltaTime;
        }
        else if (canvasGroups[0].alpha == 1 && ponerCanvasNegro)
        {
            t = 0;
            if(tES < 0.7f)
            {
                tES += Time.deltaTime;
            }
            else if(tES >= 0.7f)
            {
                SceneManager.LoadScene(newScene);
                ponerCanvasNegro = false;
            }
        }
        if(canvasGroups[2].alpha == 1)
        {
            TextoPuntos();
        }
        if (moverCamara)
        {
            canvasGroups[2].alpha = Mathf.Lerp(0, 1, t3);
            if (canvasGroups[2].alpha > 0)
            {
                t3 -= 1 * Time.deltaTime;
            }
            else if(canvasGroups[2].alpha <= 0)
            {
                camaraTransf.localPosition = new Vector3(0, 0.6f, 0);
                canvasGroups[3].alpha = 1;
                playerMovement.movimientoActivo = true;
                linterna.SetActive(true);
                duoController.gameStart = true;
                moverCamara = false;
            }
        }
        if (partyEnd)
        {
            PartyEnd();
        }
    }
    public void Reintentar()
    {
        canvasGroups[1].interactable = false;
        canvasGroups[1].blocksRaycasts = false;
        canvasGroups[0].blocksRaycasts = true;
        newScene = 1;
        ponerCanvasNegro = true;
    }
    public void VolverMenu()
    {
        canvasGroups[1].interactable = false;
        canvasGroups[1].blocksRaycasts = false;
        canvasGroups[0].blocksRaycasts = true;
        newScene = 0;
        ponerCanvasNegro = true;
    }
    public void Empezar()
    {
        t3 = 1;
        canvasGroups[2].interactable = false;
        canvasGroups[2].blocksRaycasts = false;
        Cursor.lockState = CursorLockMode.Locked;
        moverCamara = true;
    }
    public void Dificultad(int numeroNoche)
    {
        numNocheActual.text = numeroNoche.ToString("0");
        nocheSeleccionada = numeroNoche;
        switch (nocheSeleccionada)
        {
            case 1:
                images[0].sprite = sprite[4];
                images[1].sprite = sprite[0];
                images[2].sprite = sprite[4];
                break;
            case 2:
                images[0].sprite = sprite[0];
                images[1].sprite = sprite[1];
                images[2].sprite = sprite[2];
                break;
            case 3:
                images[0].sprite = sprite[3];
                images[1].sprite = sprite[1];
                images[2].sprite = sprite[2];
                break;
        }
    }
    void Awake()
    {
        canvasGroups[0].alpha = 1;
        quitarCanvasNegro = true;
        canvasGroups[2].alpha = 1;
        canvasGroups[2].interactable = true;
        canvasGroups[2].blocksRaycasts = true;
    }
    private void TextoPuntos()
    {
        textoPuntos.alpha = Mathf.Lerp(0, 1, t2);
        if(t2 <= 0)
        {
            t2Aumenta = false;
            t2 = 0;
        }
        else if(t2 >= 1)
        {
            t2Aumenta = true;
            t2 = 1;
        }
        if (t2Aumenta)
        {
            t2 -= 0.8f * Time.deltaTime;
        }
        else if (!t2Aumenta)
        {
            t2 += 0.8f * Time.deltaTime;
        }
        Debug.Log(t2);
    }
    public void PartyEnd()
    {
        if (!oneTimeBool)
        {
            linterna.SetActive(false);
            //videoPlayer.Play();
            canvasGroups[3].alpha = 0;
            canvasGroups[4].alpha = 1;
            oneTimeBool = true;
        }
        if (tImageJumpscare < 1.1f)
        {
            imageJumpscare.localScale = new Vector3(Mathf.Lerp(0.1f, 3, tImageJumpscare), Mathf.Lerp(0.1f, 3, tImageJumpscare), Mathf.Lerp(0.1f, 3, tImageJumpscare));
            tImageJumpscare += 4.4f * Time.deltaTime;
        }
        else if (tImageJumpscare >= 1.1f && !oneTimeBool2)
        {
            imageJumpscare.localScale = Vector3.zero;
            videoPlayer.Play();
            oneTimeBool2 = true;
        }
        if (videoPlayer.isPlaying == false && oneTimeBool2 == true)
        {
            canvasGroups[4].alpha = 0;
            canvasGroups[1].alpha = 1;
            canvasGroups[1].interactable = true;
            canvasGroups[1].blocksRaycasts = true;
        }
        
    }
}
