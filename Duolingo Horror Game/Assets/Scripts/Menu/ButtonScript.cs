using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] canvasGroups;
    [SerializeField] private Transform[] camaraRoad, camaraMapas;
    [SerializeField] private Transform camara;
    [SerializeField] private TMP_Text textoBoton;
    [SerializeField] private Image imageNegra;
    private bool cerrarJuego = false, moverJugar = false, cargarMapa = false;
    private float t = 0, rotX = 12.57f, rotY = 55.51f, rotZ = 0, tiempo = 0;
    private int d = 0;
    [SerializeField] private string[] textoNiveles;
    void Start()
    {
        canvasGroups[0].alpha = 1;
        canvasGroups[0].interactable = true;
        canvasGroups[0].blocksRaycasts = true;
        for (int i = 1; i < canvasGroups.Length - 1; i++)
        {
            canvasGroups[i].alpha = 0;
            canvasGroups[i].interactable = false;
            canvasGroups[i].blocksRaycasts = false;
        }
        camara.position = camaraRoad[0].position;
        camara.rotation = camaraRoad[0].rotation;
    }

    void Update()
    {
        if (cerrarJuego)
        {
            if(canvasGroups[0].alpha > 0)
            {
                Debug.Log("Quitando menu...");
                canvasGroups[0].alpha = Mathf.Lerp(1, 0, t);
                t += Time.deltaTime * 0.5f;
            }
            else if(canvasGroups[0].alpha <= 0)
            {
                Debug.Log("Saliste del juego");
                Application.Quit();
            }
        }
        if (moverJugar)
        {
            if(camara.position != camaraRoad[1].position || camara.rotation != camaraRoad[1].rotation)
            {
                camara.position = new Vector3(Mathf.MoveTowards(camara.position.x, camaraRoad[1].position.x, Time.deltaTime * 3), Mathf.MoveTowards(camara.position.y, camaraRoad[1].position.y, Time.deltaTime * 3), Mathf.MoveTowards(camara.position.z, camaraRoad[1].position.z, Time.deltaTime * 3));
                rotX = Mathf.MoveTowardsAngle(rotX, 0, 45 * Time.deltaTime);
                rotY = Mathf.MoveTowardsAngle(rotY, 180, 45 * Time.deltaTime);
                rotZ = Mathf.MoveTowardsAngle(rotZ, 0, 45 * Time.deltaTime);
                camara.eulerAngles = new Vector3(rotX, rotY, rotZ);
            }
            else if(camara.position == camaraRoad[1].position && camara.rotation == camaraRoad[1].rotation)
            {
                moverJugar = false;
                canvasGroups[1].alpha = 1;
                canvasGroups[1].interactable = true;
                canvasGroups[1].blocksRaycasts = true;
            }
        }
        if(canvasGroups[1].alpha == 1 && camara.position == camaraMapas[d].position)
        {
            canvasGroups[1].interactable = true;
            if (d != 3)
            {
                textoBoton.text = textoNiveles[d];
            }
            else if (d == 3)
            {
                textoBoton.text = "???";
            }
        }
        else if(canvasGroups[1].alpha == 1 && camara.position != camaraMapas[d].position)
        {
            canvasGroups[1].interactable = false;
            camara.position = new Vector3(Mathf.MoveTowards(camara.position.x, camaraMapas[d].position.x, Time.deltaTime * 3), Mathf.MoveTowards(camara.position.y, camaraMapas[d].position.y, Time.deltaTime * 3), Mathf.MoveTowards(camara.position.z, camaraMapas[d].position.z, Time.deltaTime * 3));
        }
        if (cargarMapa)
        {
            CargarEmpezarMapa();
        }
    }
    public void Jugar()
    {
        moverJugar = true;
        canvasGroups[0].alpha = 0;
        canvasGroups[0].interactable = false;
        canvasGroups[0].blocksRaycasts = false;
    }
    public void EmpezarMapa()
    {
        imageNegra.raycastTarget = true;
        cargarMapa = true;
    }
    public void CargarEmpezarMapa()
    {
        if (imageNegra.color.a < 1)
        {
            imageNegra.color = new Vector4(0.05f, 0.05f, 0.05f, Mathf.Lerp(0, 1, t));
            t += 1 * Time.deltaTime;
        }
        else if (imageNegra.color.a >= 1)
        {
            if (tiempo < 0.7f)
            {
                tiempo += Time.deltaTime;
            }
            else if (tiempo >= 0.7f)
            {
                SceneManager.LoadScene(d + 1);
            }
        }
    }
    public void Opciones()
    {

    }
    public void Creditos()
    {

    }
    public void Salir()
    {
        cerrarJuego = true;
        canvasGroups[0].interactable = false;
    }
    public void Flecha(bool a)
    {
        if (a)
        {
            if(d < camaraMapas.Length - 1)
            {
                d++;
            }
            else if(d >= camaraMapas.Length - 1)
            {
                d = 0;
            }
            
        }
        else if (!a)
        {
            if(d != 0)
            {
                d--;
            }
            else if(d == 0)
            {
                d = 3;
            }
            
        }
    }
}
