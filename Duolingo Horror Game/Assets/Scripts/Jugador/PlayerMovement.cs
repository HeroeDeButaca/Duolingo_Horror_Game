using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Opciones de Personaje")]
    private CharacterController characterController;
    private Rigidbody rb;
    [SerializeField] private float walkSpeed = 3, lowWalkSpeed = 2;
    [SerializeField] private float runSpeed = 5, lowRunSpeed = 3.5f;
    [SerializeField] private float gravity = 20;
    private Vector3 move = Vector3.zero;
    [SerializeField]private Vector3 spawnPoint;
    [HideInInspector] public bool movimientoActivo = false, GoSpawn = false, rotateJumpscare = false;
    [SerializeField] private bool pasoMadera, pasoEstacion, pasoOficina, pasoReto;

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

    [Header("Otros")]
    private Transform duoTransform;
    private AudioManager audioManager;

    void Start()
    {
        sliderCanvasGroup.alpha = 0;
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        duoTransform = GameObject.FindGameObjectWithTag("DuoCenter").GetComponent<Transform>();
        movimientoActivo = false;
        //Cursor.lockState = CursorLockMode.Locked;
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
            if (Input.GetKey(KeyCode.LeftShift) && playerStamina > 0 && hasRegenerated && move.z > 0.01 || Input.GetKey(KeyCode.LeftShift) && playerStamina > 0 && hasRegenerated && move.z < 0.01)
            {
                move = transform.TransformDirection(move) * runSpeed;
                weAreSprinting = true;
                if(pasoMadera && !audioManager.audioSteps.isPlaying && move != Vector3.zero && rb.velocity != Vector3.zero)
                {
                    Debug.Log("Hace sonido");
                    if (runSpeed != 3.5f)
                    {
                        audioManager.audioSteps.pitch = Random.Range(1.3f, 1.55f);
                    }
                    else if (runSpeed == 3.5f)
                    {
                        audioManager.audioSteps.pitch = Random.Range(1, 1.2f);
                    }

                    audioManager.PlaySteps(audioManager.pasosMadera);
                }
                Sprinting();
            }
            else
            {
                move = transform.TransformDirection(move) * walkSpeed;
                if (pasoMadera && !audioManager.audioSteps.isPlaying && move != Vector3.zero && rb.velocity != Vector3.zero)
                {
                    Debug.Log("Hace sonido");
                    if(walkSpeed != 2)
                    {
                        audioManager.audioSteps.pitch = Random.Range(1, 1.25f);
                    }
                    else if(walkSpeed == 2)
                    {
                        audioManager.audioSteps.pitch = Random.Range(0.8f, 0.9f);
                    }
                    
                    audioManager.PlaySteps(audioManager.pasosMadera);
                }
                else if(audioManager.audioSteps.isPlaying && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                {
                    audioManager.audioSteps.Stop();
                }
                weAreSprinting = false;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                walkSpeed = lowWalkSpeed;
                runSpeed = lowRunSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                walkSpeed = 3;
                runSpeed = 5;
            }
        }
        move.y -= gravity * Time.deltaTime;
        characterController.Move(move * Time.deltaTime);
        Stamina();
        if (GoSpawn)
        {
            GoToSpawnPoint();
        }
        if (rotateJumpscare)
        {
            cam.transform.LookAt(duoTransform.position);
        }
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
        rb.useGravity = false;
        characterController.enabled = false;
        transform.position = spawnPoint;
        if(transform.position == spawnPoint)
        {
            rb.useGravity = true;
            characterController.enabled = true;
            GoSpawn = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Duolingo"))
        {
            movimientoActivo = false;
            rotateJumpscare = true;
            characterController.radius = 1.15f;
            rb.velocity = Vector3.zero;
            move = Vector3.zero;
            gravity = 0;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
