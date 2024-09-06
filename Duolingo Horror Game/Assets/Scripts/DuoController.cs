using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DuoController : MonoBehaviour
{
    private Transform playerTransform;
    public Transform duoTransform;
    public NavMeshAgent duoNavMesh;
    private Animator duoAnimator;
    public Transform duoHidedTransform;
    [SerializeField] private Transform[] duolingoSpawns;
    private Vector3 destino;
    private int duolingoActualSpawn = 10, duolingoPreviusSpawn = 12;
    [SerializeField] private float newDuolingoSpawn, transcurredTime, timeToMove, particleTime, jumpscareTimer = 0, jpsTimer = 0, timeToStartMove;
    private bool createNewSpawn = false, duoGoSpawn, duolingoDespawned = true, countdownUp = true, jumpscare = false, oneTimeJumpscare = false, movement = false, superDuo = false;
    public bool isClimbing, duoDissapear, oneTimeBool, gameStart, isInside;
    [SerializeField] private bool sueloMadera;
    [SerializeField] private GameObject prefabParticle, duo, prefabCreado, superPrefab;
    [SerializeField] private GameObject[] climbWindows;
    private DuoLesson duoLesson;
    private GameOver gameOver;
    private AudioManager audioManager;
    [SerializeField] private BoxCollider boxCollider;
    private float minimumTimeSpawn, maximumTimeSpawn;

    // SuperDuo Materials
    [SerializeField] private Material[] superduoMaterials;
    [SerializeField] private SkinnedMeshRenderer[] duoMatParts;

    void Start()
    {
        duoTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        duoLesson = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>();
        gameOver = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameOver>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        duoNavMesh = GetComponent<NavMeshAgent>();
        duoNavMesh.enabled = false;
        duoAnimator = GetComponent<Animator>();
        minimumTimeSpawn = 13f;
        maximumTimeSpawn = 20f;
        timeToStartMove = 3;
        createNewSpawn = true;
        duoGoSpawn = true;
        gameStart = false;
        isInside = false;
        newDuolingoSpawn = Random.Range(minimumTimeSpawn, maximumTimeSpawn);
        if (sueloMadera)
        {
            for (int i = 0; i < 2; i++)
            {
                climbWindows[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (!duolingoDespawned && gameStart)
        {
            if(timeToMove < timeToStartMove)
            {
                timeToMove += Time.deltaTime;
            }
            else if(timeToMove >= timeToStartMove)
            {
                if (!isClimbing)
                {
                    duoNavMesh.enabled = true;
                    duoNavMesh.destination = playerTransform.position;
                }
                if(Vector3.Distance(destino, playerTransform.position) > 1.0f)
                {
                    duoAnimator.SetBool("walk", true);
                }
                if (sueloMadera && !climbWindows[1].active)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        climbWindows[i].SetActive(true);
                    }
                }
            }
        }
        if (duoGoSpawn && gameStart)
        {
            DuolingoSpawn();
        }

        if (createNewSpawn && gameStart)
        {
            //duolingoActualSpawn = Random.Range(0, duolingoSpawns.Length - 1);
            duolingoActualSpawn = 2;
            while (duolingoActualSpawn == duolingoPreviusSpawn)
            {
                duolingoActualSpawn = Random.Range(0, duolingoSpawns.Length - 1);
            }
            duolingoPreviusSpawn = duolingoActualSpawn;
            createNewSpawn = false;
        }
        if (duoDissapear && gameStart)
        {
            DuoDissapear();
        }
        if(jumpscare)
        {
            DuolingoJumpscare();
        }
        if (isInside)
        {
            if (sueloMadera && !audioManager.audioStepsDuo.isPlaying)
            {
                audioManager.PlayStepsDuo(audioManager.pasosMadera);
                audioManager.audioStepsDuo.pitch = Random.Range(0.9f, 1);
            }
        }
    }
    private void DuolingoSpawn()
    {
        if(newDuolingoSpawn > transcurredTime && duolingoDespawned && gameStart)
        {
            if (countdownUp)
            {
                transcurredTime += Time.deltaTime;
            }
        }
        else if(newDuolingoSpawn <= transcurredTime && gameStart)
        {
            countdownUp = false;
            if (duolingoDespawned)
            {
                createNewSpawn = true;
                duolingoDespawned = false;
            }
            duoTransform.position = duolingoSpawns[duolingoActualSpawn].position;
            transcurredTime = 0;
        }
    }
    public void DuoDissapear()
    {
        if (oneTimeBool)
        {
            if (!prefabCreado || prefabCreado == null)
            {
                if (!superDuo)
                {
                    prefabCreado = Instantiate(prefabParticle, new Vector3(duo.transform.position.x, duo.transform.position.y, duo.transform.position.z), Quaternion.identity);
                }
                else if (superDuo)
                {
                    prefabCreado = Instantiate(superPrefab, new Vector3(duo.transform.position.x, duo.transform.position.y, duo.transform.position.z), Quaternion.identity);
                }
            }
            duoTransform.position = duoHidedTransform.position;
            oneTimeBool = false;
        }
        if(particleTime < 0.98f)
        {
            particleTime += Time.deltaTime;
        }
        else if(particleTime >= 0.98f)
        {
            Destroy(prefabCreado);
            isInside = false;
            particleTime = 0;
            duoDissapear = false;
            duoNavMesh.enabled = false;
            timeToMove = 0;
            duolingoDespawned = true;
            //createNewSpawn = true;
            countdownUp = true;
            newDuolingoSpawn = Random.Range(minimumTimeSpawn, maximumTimeSpawn);
            if (sueloMadera && climbWindows[1].active)
            {
                for (int i = 0; i < 2; i++)
                {
                    climbWindows[i].SetActive(false);
                }
            }
        }
    }
    private void DuolingoJumpscare()
    {
        if (!oneTimeJumpscare)
        {
            gameStart = false;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            duoAnimator.SetBool("walk", false);
            duoAnimator.SetBool("run", false);
            duoAnimator.SetBool("jumpscare", true);
            duoNavMesh.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            oneTimeJumpscare = true;
        }
        else if (oneTimeJumpscare)
        {
            if (!movement && duoAnimator.GetBool("jumpscare") == true)
            {
                duoTransform.localPosition += new Vector3(0, 0.75f, 0);
                duoTransform.localRotation = Quaternion.Euler(0, duoTransform.localRotation.y + 15, 0);
                movement = true;
            }
            else if (movement)
            {
                if(jumpscareTimer < 1)
                {
                    jumpscareTimer += Time.deltaTime;
                }
                else if(jumpscareTimer >= 1)
                {
                    gameOver.partyEnd = true;
                }
            }
        }
        duoTransform.LookAt(new Vector3(0, playerTransform.position.y, 0));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //boxCollider.enabled = true;
            jumpscare = true;
            duoLesson.canvasLeccion.alpha = 0;
            duoLesson.canvasLeccion.blocksRaycasts = false;
        }
    }
    public void ChangeToSuper()
    {
        for (int i = 0; i < 8; i++)
        {
            if(i < 6)
            {
                duoMatParts[i].material = superduoMaterials[i];
            }
            else if(i >= 6)
            {
                duoMatParts[i].material = superduoMaterials[5];
            }
        }
        duoNavMesh.speed = 3.5f;
        duoNavMesh.acceleration = 7.5f;
        minimumTimeSpawn = 9f;
        maximumTimeSpawn = 15f;
        timeToStartMove = 1.5f;
        superDuo = true;
        newDuolingoSpawn = Random.Range(minimumTimeSpawn, maximumTimeSpawn);
    }
}
