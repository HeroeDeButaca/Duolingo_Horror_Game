using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class GameOver : MonoBehaviour
{
    public CanvasGroup[] canvasGroups;
    private bool quitarCanvasNegro = true, ponerCanvasNegro = false, t2Aumenta = true, moverCamara = false, oneTimeBool = false, oneTimeBool2 = false, audioJump = false;
    public bool partyEnd = false, leccionCompletada = false, botonesCargados = false;
    private int newScene, actualScene, modificador;
    public bool[] nochesSuperadas = new bool[9], cartasObtenidas = new bool[8];
    //Datos para Guardado
    public int nocheSeleccionada = 0, carta = 0;
    public bool nocheSuperada = false, cartaObtenida = false;
    //FIN Datos para Guardado
    private float t = 0, tES, t2 = 0, t3, t4 = 0, tImageJumpscare = 0, alphaNoche, scale = 0.8f, t5 = 0;
    [SerializeField] private Image[] images;
    [SerializeField] private RawImage[] objects;
    [SerializeField] private Sprite[] sprite;
    [SerializeField] private Transform camaraTransf, imageJumpscare;
    [SerializeField] private TMP_Text textoPuntos, numNocheActual, segundos;
    private PlayerMovement playerMovement;
    private DuoController duoController;
    private PhantomDuoController phantomDuo;
    [SerializeField] private GameObject linterna;
    [SerializeField] private GameObject[] cartasDuo;
    [SerializeField] private Button[] botonNoches;
    [SerializeField] private VideoPlayer videoPlayer;
    private AudioManager audioManager;
    [SerializeField] private Notificaciones notifications;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        duoController = GameObject.FindGameObjectWithTag("Duolingo").GetComponent<DuoController>();
        phantomDuo = GameObject.FindGameObjectWithTag("PhantomDuo").GetComponent<PhantomDuoController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        images[1].sprite = sprite[0];
        nocheSeleccionada = 0;
        alphaNoche = 0.5f;
        scale = 0.8f;
        modificador = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>().modificador;
        DatosJuego datosJuego = DatosJugador.LoadPlayerData();
        if (DatosJugador.fileExists)
        {
            for (int i = 0; i < 9; i++)
            {
                nochesSuperadas[i] = datosJuego.nochesSuperadas[i];
            }
            for (int i = 0; i < 8; i++)
            {
                cartasObtenidas[i] = datosJuego.cartasObtenidas[i];
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canvasGroups[2].alpha == 1)
        {
            ponerCanvasNegro = true;
            canvasGroups[2].interactable = false;
            canvasGroups[2].blocksRaycasts = false;
        }
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
                if(nocheSeleccionada != 2)
                {
                    canvasGroups[3].alpha = 1;
                }
                else if(nocheSeleccionada == 2)
                {
                    canvasGroups[7].alpha = 1;
                }
                playerMovement.movimientoActivo = true;
                linterna.SetActive(true);
                duoController.gameStart = true;
                if(nocheSeleccionada >= 1)
                {
                    phantomDuo.gameStart = true;
                    notifications.gameStart = true;
                    if(nocheSeleccionada == 2)
                    {
                        duoController.ChangeToSuper();
                    }
                }
                moverCamara = false;
            }
        }
        if (partyEnd)
        {
            PartyEnd();
        }
        if (leccionCompletada)
        {
            leccionTerminada();
        }
        if (!botonesCargados)
        {
            botones();
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
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>().assignarArray();
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>().empezarTiempo = true;
        cartasDuo[nocheSeleccionada - modificador].SetActive(true);
    }
    public void Dificultad(int numeroNoche)
    {
        numNocheActual.text = (numeroNoche + 1).ToString("0");
        nocheSeleccionada = numeroNoche;
        switch (nocheSeleccionada)
        {
            case 0:
                images[0].sprite = sprite[4];
                images[1].sprite = sprite[0];
                images[2].sprite = sprite[4];
                break;
            case 1:
                images[0].sprite = sprite[0];
                images[1].sprite = sprite[1];
                images[2].sprite = sprite[2];
                break;
            case 2:
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
    }
    public void PartyEnd()
    {
        if (!oneTimeBool)
        {
            linterna.SetActive(false);
            canvasGroups[3].alpha = 0;
            canvasGroups[4].alpha = 1;
            canvasGroups[5].alpha = 0;
            canvasGroups[6].alpha = 0;
            oneTimeBool = true;
        }
        if (tImageJumpscare < 0.95f)
        {
            imageJumpscare.localScale = new Vector3(Mathf.Lerp(0.1f, 3.15f, tImageJumpscare), Mathf.Lerp(0.1f, 3.15f, tImageJumpscare), Mathf.Lerp(0.1f, 3.15f, tImageJumpscare));
            tImageJumpscare += 4.4f * Time.deltaTime;
            if (!audioJump && tImageJumpscare >= 0.1f)
            {
                audioManager.PlayJumpscare();
                audioJump = true;
            }
        }
        else if (tImageJumpscare >= 0.95f && !oneTimeBool2)
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
    private void leccionTerminada()
    {
        canvasGroups[6].alpha = 1;
        canvasGroups[6].interactable = false;
        canvasGroups[6].blocksRaycasts = true;
        if(t4 < 1)
        {
            objects[0].color = new Vector4(1, 1, 1, alphaNoche);
            alphaNoche = Mathf.Lerp(0.5f, 1, t4);
            t4 += Time.deltaTime * 2;
            if (cartaObtenida)
            {
                objects[1].color = new Vector4(1, 1, 1, alphaNoche);
            }
        }
        else if(t4 >= 1)
        {
            objects[0].gameObject.GetComponent<Animator>().SetBool("crecer", true);
            if (cartaObtenida)
            {
                objects[1].gameObject.GetComponent<Animator>().SetBool("crecer", true);
            }
            canvasGroups[6].interactable = true;
        }
    }
    public void ResetOSalirLeccion(int escenaNueva)
    {
        nochesSuperadas[nocheSeleccionada] = true;
        if (cartasObtenidas[nocheSeleccionada])
        {
            cartasObtenidas[nocheSeleccionada] = true;
        }
        else if (!cartasObtenidas[nocheSeleccionada])
        {
            cartasObtenidas[nocheSeleccionada] = cartaObtenida;
        }
        ponerCanvasNegro = true;
        t = 0;
        tES = 0;
        newScene = escenaNueva;
        DatosJugador.SaveData(this);
        Debug.Log("Datos Guardados");
    }
    private void OnLevelWasLoaded(int level)
    {
        actualScene = level;
        Debug.Log(level);
    }
    private void botones(int i = 0)
    {
        while(i < 2)
        {
            Debug.Log(i);
            if (nochesSuperadas[i + modificador])
            {
                botonNoches[i + 1].interactable = true;
            }
            else if (!nochesSuperadas[i + modificador])
            {
                botonNoches[i + 1].interactable = false;
            }
            i++;
        }
        botonesCargados = true;
    }
}
