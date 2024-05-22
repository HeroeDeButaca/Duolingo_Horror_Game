using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Opciones de Personaje")]
    private CharacterController characterController;
    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private float gravity = 20;
    private Vector3 move = Vector3.zero;
    [SerializeField]private Vector3 spawnPoint;
    [HideInInspector]public bool movimientoActivo = true;

    [Header("Opciones de Camara")]
    public Camera cam;
    [SerializeField] private float mouseHorizontal = 3;
    [SerializeField] private float mouseVertical = 2;
    [SerializeField] private float minRotation = -65;
    [SerializeField] private float maxRotation = 60;
    private float h_mouse, v_mouse;

    //Stamina
    [Header("Parametros Stamina")]
    public float playerStamina = 100;
    [SerializeField] private float maxStamina = 100;
    [HideInInspector]public bool hasRegenerated = true, weAreSprinting = false;

    [Header("Parametros de regeneracion de Stamina")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)] [SerializeField] private float staminaRegen = 0.5f;

    [Header("Elementos UI Stamina")]
    [SerializeField] private Image staminaProgressUI = null, staminaProgressUI2 = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    void Start()
    {
        sliderCanvasGroup.alpha = 0;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (movimientoActivo)
        {
            h_mouse = mouseHorizontal * Input.GetAxis("Mouse X");
            v_mouse += mouseVertical * Input.GetAxis("Mouse Y");

            v_mouse = Mathf.Clamp(v_mouse, minRotation, maxRotation);
            cam.transform.localEulerAngles = new Vector3(-v_mouse, 0, 0);
            transform.Rotate(0, h_mouse, 0);

            move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (Input.GetKey(KeyCode.LeftShift) && playerStamina > 0 && hasRegenerated)
            {
                move = transform.TransformDirection(move) * runSpeed;
                weAreSprinting = true;
                Sprinting();
            }
            else
            {
                move = transform.TransformDirection(move) * walkSpeed;
                weAreSprinting = false;
            }
            move.y -= gravity * Time.deltaTime;
            characterController.Move(move * Time.deltaTime);
        }
        Stamina();
    }
    private void Stamina()
    {
        //Regen Stamina
        if (!weAreSprinting)
        {
            if(playerStamina <= maxStamina - 0.01)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina(1);
                if(playerStamina >= maxStamina - 0.01)
                {
                    sliderCanvasGroup.alpha = 0;
                    hasRegenerated = true;
                }
            }
        }
    }
    private void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);
            if(playerStamina <= 0)
            {
                hasRegenerated = false;
                sliderCanvasGroup.alpha = 0;
            }
        }
    }
    private void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        staminaProgressUI2.fillAmount = playerStamina / maxStamina;
        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
    public void GoToSpawnPoint()
    {
        Debug.Log("Jugador va al Spawn");
        transform.position = spawnPoint;
    }
}
