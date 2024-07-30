using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class DuoLesson : MonoBehaviour
{
    [Header("Leccion Duolingo")]
    public string[,] stringLeccion;
    private string[,] stringLeccion1 = new string[5,5]
    {
        {"Hola", "me", "llamo", "una", "Duo" },
        {"gustan", "conejos", "Me", "los", "las" },
        {"conejos", "comer", "como", "�l", "Yo" },
        {"comer", "Tu", "conejos", "comes", "�l" },
        {"gusta", "humanos", "tambi�n", "comer", "Me" }
    };
    private string[,] stringLeccion2 = new string[6, 7]
    {
        {"Let's", "yesterday's", "with", "for", "Start", "talk", "continue"},
        {"ti", "comer", "gusta", "A", "humano", "te", "humanos"},
        {"buen", "muy", "genial", "Ten�is", "olor", "sabor", "Obteneis"},
        {"mi", "Dejate", "por", "favor", "comer", "para", "por"},
        {"have", "only", "a", "please", "to", "You", "wait"},
        {"esta", "tu", "termines", "este", "lecci�n", "favor", "No"}
    };
    private string[] stringIngles;
    private string[] stringIngles1 =
    {
        "Hello, my name is Duo",
        "I like rabbits",
        "I eat rabbits",
        "Do you eat rabbits?",
        "I like to eat humans too"
    };
    private string[] stringIngles2 =
    {
        "Continuemos con la charla de ayer",
        "Do you like to eat humans?",
        "You have very good taste",
        "Let yourself be eaten by me, please",
        "Solo tienes que esperar",
        "Don't finish this lesson!"
    };
    private string[] idAns;
    private string[] idAns1 = {"Hola me llamo Duo ", "Me gustan los conejos ", "Yo como conejos ", "Tu comes conejos ", "Me gusta comer humanos tambi�n "};
    private string[] idAns2 = { "Let's continue with yesterday's talk ", "A ti te gusta comer humanos ", "Ten�is muy buen sabor ", "Dejate comer por mi por favor ", "You only have to wait ", "No termines esta lecci�n "};

    public string idIntroducido, fraseIntroducida;
    [SerializeField] private int actLevel = 0, erroresCometidos = 0;
    private bool esCorrecto = false, esIncorrecto = false, cubosDestruidos, invocarCubos = false, empezarLeccion = false;

    [Header("Canvas")]
    public CanvasGroup canvasLeccion, canvasLinterna;
    [SerializeField] private TMP_Text textoIngles, textoMinutos, textoSegundos, textoErrores;
    public int palabrasIntroducidas, palabrasMax;
    [SerializeField] private Image barraLeccion;
    [SerializeField] private GameObject panelCargando, leccionCompletada, lineasPalabras, finNivel;
    private float tiempoCargando, minutos = 0, segundos = 0;
    [SerializeField] private float tiempoDeCarga;
    private bool cargando;
    public bool empezarTiempo = false;
    [SerializeField] private Button resetButton, comprobarButton, cerrarButton;

    [Header("Cubos Leccion")]
    [SerializeField]private GameObject padreLeccion;
    [SerializeField]private GameObject prefabCubo;
    public List<GameObject> cubosEnPantalla = new List<GameObject>();
    public List<GameObject> palabrasEnUso = new List<GameObject>();

    [Header("Otros")]
    private PlayerMovement playerMovement;
    private RaycastPlayer raycastPlayer;
    private AudioManager audioManager;
    private GameOver gameOver;
    public int modificador, total;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        gameOver = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameOver>();
        raycastPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastPlayer>();
        invocarCubos = false;
    }

    void Update()
    {
        if (empezarLeccion)
        {
            if (actLevel == idAns.Length)
            {
                barraLeccion.fillAmount = 1;
            }
            else if (actLevel == 0)
            {
                barraLeccion.fillAmount = 0;
            }
            else
            {
                barraLeccion.fillAmount = (float)actLevel / (float)idAns.Length;
            }
        }
        InvocarCubos();
        Cargando();
        if (empezarTiempo)
        {
            segundos += Time.deltaTime;
            if(segundos > 59.9f)
            {
                minutos += 1;
                segundos = 0;
            }
        }
    }
    private void InvocarCubos()
    {
        if (invocarCubos && actLevel <= idAns.Length - 1)
        {
            textoIngles.text = stringIngles[actLevel];
            for (int i = 0; i < palabrasMax; i++)
            {
                cubosEnPantalla.Add(Instantiate(prefabCubo) as GameObject);
                cubosEnPantalla[i].transform.parent = padreLeccion.transform;
                cubosEnPantalla[i].GetComponentInChildren<TMP_Text>().text = stringLeccion[actLevel, i];
                switch (i)
                {
                    case 0:
                        cubosEnPantalla[i].GetComponent<RectTransform>().localPosition = new Vector2(-587.5f, -221);
                        cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().posicionInicial = new Vector2(-587.5f, -221);
                        break;
                    case 1:
                        cubosEnPantalla[i].GetComponent<RectTransform>().localPosition = new Vector2(-427.5f, -221);
                        cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().posicionInicial = new Vector2(-427.5f, -221);
                        break;
                    case 2:
                        cubosEnPantalla[i].GetComponent<RectTransform>().localPosition = new Vector2(-267.5f, -221);
                        cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().posicionInicial = new Vector2(-267.5f, -221);
                        break;
                    case 3:
                        cubosEnPantalla[i].GetComponent<RectTransform>().localPosition = new Vector2(-107.5f, -221);
                        cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().posicionInicial = new Vector2(-107.5f, -221);
                        break;
                    case 4:
                        cubosEnPantalla[i].GetComponent<RectTransform>().localPosition = new Vector2(52.5f, -221);
                        cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().posicionInicial = new Vector2(52.5f, -221);
                        break;
                    case 5:
                        cubosEnPantalla[i].GetComponent<RectTransform>().localPosition = new Vector2(212.5f, -221);
                        cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().posicionInicial = new Vector2(212.5f, -221);
                        break;
                    case 6:
                        cubosEnPantalla[i].GetComponent<RectTransform>().localPosition = new Vector2(372.5f, -221);
                        cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().posicionInicial = new Vector2(372.5f, -221);
                        break;
                }
                cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().id = i;
                cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().palabraBoton = stringLeccion[actLevel, i];
                if(i <= palabrasMax)
                {
                    invocarCubos = false;
                }
            }
        }
    }
    public void CerrarLeccion()
    {
        canvasLeccion.alpha = 0;
        canvasLeccion.interactable = false;
        canvasLinterna.alpha = 1;
        playerMovement.movimientoActivo = true;
        Cursor.lockState = CursorLockMode.Locked;
        raycastPlayer.leccionAbierta = false;
        Cursor.visible = false;
    }
    public void ResetPalabras()
    {
        idIntroducido = null;
        fraseIntroducida = null;
        palabrasIntroducidas = 0;
        for(int o = 0; o < palabrasMax; o++)
        {
            cubosEnPantalla[o].GetComponent<Boton_palabra_leccion>().reordenar_palabras = true;
            cubosEnPantalla[o].GetComponent<Boton_palabra_leccion>().esRespuesta = false;
            cubosEnPantalla[o].GetComponent<Boton_palabra_leccion>().noCambiar = false;
            cubosEnPantalla[o].GetComponent<Boton_palabra_leccion>().Reordenar_palabras();
        }
    }
    private void DestruirCubos()
    {
        idIntroducido = null;
        fraseIntroducida = null;
        palabrasIntroducidas = 0;
        for(int d = 0; d < palabrasMax; d++)
        {
            Destroy(cubosEnPantalla[d].gameObject);
            if(d < palabrasMax)
            {
                cubosDestruidos = true;
                Debug.Log("Cubos destruidos");
            }
        }
    }
    public void Boton_Comprobar()
    {
        if(fraseIntroducida == idAns[actLevel])
        {
            Debug.Log("Continuar leccion" + actLevel);
            esCorrecto = true;
            audioManager.PlaySFX(audioManager.correctoLesson);
            Comprobar();
        }
        else if(fraseIntroducida != idAns[actLevel])
        {
            erroresCometidos += 1;
            esIncorrecto = true;
            audioManager.PlaySFX(audioManager.incorrectoLesson);
            Comprobar();
        }
    }
    private void Comprobar()
    {
        if(esCorrecto)
        {
            actLevel++;
            DestruirCubos();
            if (cubosDestruidos)
            {
                cubosEnPantalla.Clear();
                if(actLevel <= idAns.Length - 1)
                {
                    invocarCubos = true;
                    cargando = true;
                }
                else if(actLevel > idAns.Length - 1)
                {
                    textoIngles.text = "�Lecci�n Completada!";
                    GameObject.FindGameObjectWithTag("Duolingo").GetComponent<DuoController>().gameStart = false;
                    GameObject.FindGameObjectWithTag("PhantomDuo").GetComponent<PhantomDuoController>().gameStart = false;
                    if (GameObject.FindGameObjectWithTag("Duolingo").GetComponent<NavMeshAgent>().enabled)
                    {
                        GameObject.FindGameObjectWithTag("Duolingo").GetComponent<NavMeshAgent>().destination = gameObject.transform.position;
                    }
                    lineasPalabras.SetActive(false);
                    leccionCompletada.SetActive(true);
                    resetButton.interactable = false;
                    comprobarButton.interactable = false;
                    comprobarButton.image.color = new Vector4(1, 1, 1, 0);
                    cerrarButton.interactable = false;
                    finNivel.SetActive(true);
                }
                esCorrecto = false;
                cubosDestruidos = false;
            }
        }
        else if (esIncorrecto)
        {
            Debug.Log("Es incorrecto");
            DestruirCubos();
            if (cubosDestruidos)
            {
                cubosEnPantalla.Clear();
                invocarCubos = true;
                esCorrecto = false;
                cubosDestruidos = false;
            }
        }
    }
    private void Cargando()
    {
        if(tiempoCargando < tiempoDeCarga && cargando)
        {
            panelCargando.SetActive(true);
            tiempoCargando += Time.deltaTime;
            Debug.Log("Cargando...");
            resetButton.interactable = false;
            comprobarButton.interactable = false;
        }
        else if (tiempoCargando >= tiempoDeCarga && cargando)
        {
            panelCargando.SetActive(false);
            tiempoCargando = 0;
            cargando = false;
            resetButton.interactable = true;
            comprobarButton.interactable = true;
        }
    }
    public void finNoche()
    {
        audioManager.PlaySFX(audioManager.lessonComplete);
        empezarTiempo = false;
        if(minutos < 10)
        {
            textoMinutos.text = minutos.ToString("0:");
        }
        else if(minutos >= 10)
        {
            textoMinutos.text = minutos.ToString("00:");
        }
        textoSegundos.text = segundos.ToString("00");
        if(erroresCometidos == 0)
        {
            textoErrores.text = "No tuviste ning�n error en esta lecci�n";
        }
        else if(erroresCometidos >= 1)
        {
            textoErrores.text = ("Errores cometidos: " + erroresCometidos);
        }
        canvasLeccion.alpha = 0;
        canvasLeccion.interactable = false;
        canvasLeccion.blocksRaycasts = false;
        gameOver.leccionCompletada = true;
        gameOver.nocheSuperada = true;
    }
    public void assignarArray()
    {
        total += modificador + gameOver.nocheSeleccionada;
        switch (total)
        {
            case 0:
                stringLeccion = new string[5, 5];
                stringLeccion = stringLeccion1;
                idAns = idAns1;
                stringIngles = stringIngles1;
                palabrasMax = 5;
                break;
            case 1:
                stringLeccion = new string[6, 7];
                stringLeccion = stringLeccion2;
                stringIngles = stringIngles2;
                idAns = idAns2;
                palabrasMax = 7;
                break;
        }
        empezarLeccion = true;
        invocarCubos = true;
    }
}
