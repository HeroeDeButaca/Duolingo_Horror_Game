using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DuoController : MonoBehaviour
{
    private Transform playerTransform;
    public Transform duoTransform;
    private NavMeshAgent duoNavMesh;
    private Animator duoAnimator;
    public Transform duoHidedTransform;
    [SerializeField] private Transform[] duolingoSpawns;
    private Vector3 destino;
    private int duolingoActualSpawn = 10, duolingoPreviusSpawn = 12;
    [SerializeField] private float newDuolingoSpawn, transcurredTime, timeToMove, particleTime, jumpscareTimer = 0, jpsTimer = 0;
    private bool createNewSpawn = false, duoGoSpawn, duolingoDespawned = true, countdownUp = true, jumpscare = false, oneTimeJumpscare = false, movement = false;
    public bool isClimbing, duoDissapear, oneTimeBool, gameStart;
    [SerializeField] private GameObject prefabParticle, duo;
    private DuoLesson duoLesson;
    private GameOver gameOver;
    void Start()
    {
        duoTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        duoLesson = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DuoLesson>();
        gameOver = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameOver>();
        duoNavMesh = GetComponent<NavMeshAgent>();
        duoNavMesh.enabled = false;
        duoAnimator = GetComponent<Animator>();
        //newDuolingoSpawn = Random.Range(12f, 25f);
        newDuolingoSpawn = 10;
        createNewSpawn = true;
        duoGoSpawn = true;
        gameStart = false;
    }

    void Update()
    {
        if (!duolingoDespawned && gameStart)
        {
            if(timeToMove < 3)
            {
                timeToMove += Time.deltaTime;
            }
            else if(timeToMove >= 3)
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
            }
        }
        if (duoGoSpawn && gameStart)
        {
            DuolingoSpawn();
        }

        if (createNewSpawn && gameStart)
        {
            //duolingoActualSpawn = Random.Range(0, duolingoSpawns.Length - 1);
            duolingoActualSpawn = 0;
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
            duoTransform.position = duoHidedTransform.position;
            oneTimeBool = false;
        }
        if(particleTime < 0.98f)
        {
            particleTime += Time.deltaTime;
        }
        else if(particleTime >= 0.98f)
        {
            Destroy(GameObject.Find("DuoDissapear(Clone)"));
            particleTime = 0;
            duoDissapear = false;
            duoNavMesh.enabled = false;
            timeToMove = 0;
            duolingoDespawned = true;
            createNewSpawn = true;
            countdownUp = true;
            //newDuolingoSpawn = Random.Range(10f, 20f);
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
        if (collision.gameObject.CompareTag("Disparo"))
        {
            Debug.Log("Disparo recibido");
            oneTimeBool = true;
            duoDissapear = true;
            duoNavMesh.enabled = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colision con Player");
            jumpscare = true;
            duoLesson.canvasLeccion.alpha = 0;
            duoLesson.canvasLeccion.blocksRaycasts = false;
        }
    }
}
