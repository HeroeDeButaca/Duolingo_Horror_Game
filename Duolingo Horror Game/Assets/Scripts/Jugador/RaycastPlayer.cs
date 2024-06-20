using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastPlayer : MonoBehaviour
{
    public float distancia = 3, distanciaLinterna = 12;
    [SerializeField] private CanvasGroup canvasLeccion, canvasLinterna;
    [SerializeField] private Image Interaccion;
    [SerializeField] private Sprite vacioSprite, puertaSprite, leccionSprite, linternaSprite;
    private PlayerMovement playerMovement;
    [SerializeField] private Linterna linterna;
    [SerializeField] private GameObject prefabParticle, duo;
    private DuoController duoController;
    public bool leccionAbierta;
    void Start()
    {
        duoController = GameObject.FindGameObjectWithTag("Duolingo").GetComponent<DuoController>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        leccionAbierta = false;
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia))
        {
            //Debug.Log(hit.collider.tag);
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
                if (!leccionAbierta)
                {
                    Interaccion.sprite = leccionSprite;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    leccionAbierta = true;
                    playerMovement.movimientoActivo = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Interaccion.sprite = vacioSprite;
                    canvasLeccion.alpha = 1;
                    canvasLeccion.interactable = true;
                    canvasLinterna.alpha = 0;
                }
            }
            else if (hit.collider.CompareTag("Pilas"))
            {
                Interaccion.sprite = linternaSprite;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    linterna.cargas = linterna.maxCargas;
                    Interaccion.sprite = vacioSprite;
                    linterna.CargasBateria();
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
        }
    }
}
