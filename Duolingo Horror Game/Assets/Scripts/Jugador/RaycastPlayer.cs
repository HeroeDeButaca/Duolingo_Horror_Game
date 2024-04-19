using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastPlayer : MonoBehaviour
{
    public float distancia = 3;
    [SerializeField] private CanvasGroup canvasLeccion;
    [SerializeField] private Image Interaccion;
    [SerializeField] private Sprite vacioSprite, puertaSprite, leccionSprite;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia))
        {
            if (hit.collider.CompareTag("Puerta"))
            {
                Interaccion.sprite = puertaSprite;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.transform.GetComponent<Puertas>().ChangeDoorState();
                }
            }
            else if (hit.collider.CompareTag("Ordenador_leccion"))
            {
                Interaccion.sprite = leccionSprite;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerMovement.movimientoActivo = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Interaccion.sprite = vacioSprite;
                    canvasLeccion.alpha = 1;
                    canvasLeccion.interactable = true;
                }
            }
        }
        else
        {
            Interaccion.sprite = vacioSprite;
        }
        //Debug.Log(hit.collider.tag);
    }
}
