using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuoLesson : MonoBehaviour
{
    [Header("Leccion Duolingo")]
    private string[,] stringLeccion =
    {
        {"Hola", "me", "llamo", "una", "Duo" },
        {"gustan", "conejos", "Me", "los", "las" },
        {"conejos", "comer", "como", "Él", "Yo" },
        {"comer", "Tu", "conejos", "comes", "Él" }
    };
    private string[] stringIngles =
    {
        "Hello, my name is Duo",
        "I like rabbits",
        "I eat rabbits",
        "Do you eat rabbits?"
    };
    private string[] idAns = {"0124", "2031", "420", "132"};
    public string idIntroducido;
    [SerializeField] private int actLevel = 0;
    private bool esCorrecto = false, esIncorrecto = false, cubosDestruidos, invocarCubos = false;

    [Header("Canvas")]
    [SerializeField] private CanvasGroup canvasLeccion;
    [SerializeField] private TMP_Text textoIngles;
    public int palabrasIntroducidas;
    [SerializeField] private Image barraLeccion;
    [SerializeField] private GameObject panelCargando;
    private float tiempoCargando;
    private bool cargando;

    [Header("Cubos Leccion")]
    [SerializeField]private GameObject padreLeccion;
    [SerializeField]private GameObject prefabCubo;
    public List<GameObject> cubosEnPantalla = new List<GameObject>();
    public List<GameObject> palabrasEnUso = new List<GameObject>();

    [Header("Otros")]
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        invocarCubos = true;
        //InvocarCubos();
    }

    void Update()
    {
        if (actLevel == idAns.Length)
        {
            barraLeccion.fillAmount = 1;
        }
        else if (actLevel == 0)
        {
            barraLeccion.fillAmount = 0;
            //finLeccion = true;
        }
        else
        {
            barraLeccion.fillAmount = (float)actLevel / (float)idAns.Length;
        }
        InvocarCubos();
        Cargando();
    }
    private void InvocarCubos()
    {
        if (invocarCubos)
        {
            textoIngles.text = stringIngles[actLevel];
            for (int i = 0; i < 5; i++)
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
                }
                cubosEnPantalla[i].GetComponent<Boton_palabra_leccion>().id = i;
                if(i <= 5)
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
        playerMovement.movimientoActivo = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ResetPalabras()
    {
        idIntroducido = null;
        palabrasIntroducidas = 0;
        for(int o = 0; o < 5; o++)
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
        palabrasIntroducidas = 0;
        for(int d = 0; d < 5; d++)
        {
            Destroy(cubosEnPantalla[d].gameObject);
            if(d < 5)
            {
                cubosDestruidos = true;
                Debug.Log("Cubos destruidos");
            }
        }
    }
    public void Boton_Comprobar()
    {
        if(idIntroducido == idAns[actLevel])
        {
            Debug.Log("Continuar leccion" + actLevel);
            esCorrecto = true;
            Comprobar();
        }
        else if(idIntroducido != idAns[actLevel])
        {
            esIncorrecto = true;
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
                    //esCorrecto = false;
                    //cubosDestruidos = false;
                    cargando = true;
                }
                else if(actLevel > idAns.Length - 1)
                {
                    textoIngles.text = "¡Lección Completada!";
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
        if(tiempoCargando < 3.5f && cargando)
        {
            panelCargando.SetActive(true);
            tiempoCargando += Time.deltaTime;
            Debug.Log("Cargando...");
        }
        else if (tiempoCargando >= 3.5f && cargando)
        {
            panelCargando.SetActive(false);
            tiempoCargando = 0;
            cargando = false;
        }
    }
}
