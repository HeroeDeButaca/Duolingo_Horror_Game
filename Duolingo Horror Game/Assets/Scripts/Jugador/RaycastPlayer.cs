using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastPlayer : MonoBehaviour
{
    public float distancia = 3, distanciaLinterna = 12;
    [SerializeField] private CanvasGroup canvasLeccion, canvasLinterna, canvasSuperLinterna;
    [SerializeField] private Image Interaccion;
    [SerializeField] private Sprite vacioSprite, puertaSprite, leccionSprite, linternaSprite, cartaSprite, botonSprite;
    private PlayerMovement playerMovement;
    [SerializeField] private Linterna linterna;
    [SerializeField] private GameObject prefabParticle, duo;
    private AudioManager audioManager;
    private GameOver gameOver;
    public bool leccionAbierta;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        gameOver = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameOver>();
        leccionAbierta = false;
    }
    void FixedUpdate()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.green, distancia);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Puerta"))
            {
                if (hit.collider.GetComponent<Puertas>().isInteractable)
                {
                    Interaccion.sprite = puertaSprite;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.transform.GetComponent<Puertas>().ChangeDoorState();
                }
            }
            else if (hit.collider.CompareTag("Ordenador_leccion"))
            {
                if (!leccionAbierta && Interaccion.sprite != leccionSprite)
                {
                    Interaccion.sprite = leccionSprite;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    leccionAbierta = true;
                    playerMovement.movimientoActivo = false;
                    playerMovement.move = Vector3.zero;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Interaccion.sprite = vacioSprite;
                    canvasLeccion.alpha = 1;
                    canvasLeccion.interactable = true;
                    canvasLinterna.alpha = 0;
                    canvasSuperLinterna.alpha = 0;
                }
            }
            else if (hit.collider.CompareTag("Pilas"))
            {
                Interaccion.sprite = linternaSprite;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    linterna.cargas = linterna.maxCargas;
                    audioManager.PlaySFX(audioManager.recargaLinterna);
                    Interaccion.sprite = vacioSprite;
                    linterna.CargasBateria();
                }
            }
            else if (hit.collider.CompareTag("CartaDuo"))
            {
                Interaccion.sprite = cartaSprite;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gameOver.cartaObtenida = true;
                    Interaccion.sprite = vacioSprite;
                    hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
            else if (hit.collider.CompareTag("Space_button"))
            {
                Interaccion.sprite = botonSprite;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GameObject.FindGameObjectWithTag("UFO").GetComponent<Animator>().SetBool("Pulsar", true);
                    GameObject.FindGameObjectWithTag("UFO").GetComponent<AlienAttack>().AlienButton();
                }
            }
            else if (hit.collider.CompareTag("Untagged"))
            {
                Interaccion.sprite = vacioSprite;
            }
        }
        else
        {
            Interaccion.sprite = vacioSprite;
            Debug.Log("Activado Else");
        }
    }
}
