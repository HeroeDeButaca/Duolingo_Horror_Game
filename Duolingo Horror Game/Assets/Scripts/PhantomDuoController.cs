using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomDuoController : MonoBehaviour
{
    [SerializeField] private int actualSpawn, lastSpawn = 13;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private float timeToReSpawn = 0, timeNewRespawn = 20;

    // Jumpscare
    [SerializeField] private CanvasGroup canvasPDjumpscare;
    [SerializeField] private Transform imageJumpscare;
    private float tImageJumpscare;
    [HideInInspector] public bool pdJumpscare = false, oneTimeBool = true, oneTimeBool2 = true, gameStart = false;
    private bool audioJump = false;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        gameObject.transform.position = spawns[0].position;
        Debug.Log("La suma es: " + (spawns.Length - 1));
    }

    void Update()
    {
        if (pdJumpscare && gameStart)
        {
            PhantomDuoJumpscare();
            if (oneTimeBool)
            {
                gameObject.transform.position = spawns[0].position;
                oneTimeBool = false;
            }
        }
        if(gameObject.transform.position == spawns[0].position && gameStart)
        {
            reSpawn();
            if (oneTimeBool2)
            {
                actualSpawn = Random.Range(1, spawns.Length);
                while (lastSpawn == actualSpawn)
                {
                    actualSpawn = Random.Range(1, spawns.Length);
                    Debug.Log("Uso while");
                }
                
                oneTimeBool2 = false;
                Debug.Log("Ha salido: " + actualSpawn);
            }
        }
    }
    private void PhantomDuoJumpscare()
    {
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
        else if (tImageJumpscare >= 0.95f)
        {
            canvasPDjumpscare.alpha = 0;
            imageJumpscare.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            tImageJumpscare = 0;
            oneTimeBool2 = true;
            pdJumpscare = false;
        }
    }
    private void reSpawn()
    {
        if (timeToReSpawn < timeNewRespawn)
        {
            timeToReSpawn += Time.deltaTime;
        }
        else if (timeToReSpawn >= timeNewRespawn)
        {
            gameObject.transform.position = spawns[actualSpawn].position;
            gameObject.transform.localRotation = spawns[actualSpawn].localRotation;
            timeNewRespawn = Random.Range(14f, 20f);
            lastSpawn = actualSpawn;
            timeToReSpawn = 0;
        }
    }
}
