using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DuoController : MonoBehaviour
{
    private Transform duoTransform, playerTransform;
    private NavMeshAgent duoNavMesh;
    private Animator duoAnimator;
    [SerializeField]private Transform duoHidedTransform;
    [SerializeField]private Transform[] duolingoSpawns;
    private Vector3 destino;
    private int duolingoActualSpawn = 10, duolingoPreviusSpawn = 0;
    [SerializeField]private float newDuolingoSpawn, transcurredTime, timeToMove;
    private bool createNewSpawn = false, duoGoSpawn, duolingoDespawned = true, countdownUp = true;
    void Start()
    {
        duoTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        duoNavMesh = GetComponent<NavMeshAgent>();
        destino = duoNavMesh.destination;
        duoAnimator = GetComponent<Animator>();
        //newDuolingoSpawn = Random.Range(12f, 25f);
        newDuolingoSpawn = 10;
        createNewSpawn = true;
        duoGoSpawn = true;
    }

    void Update()
    {
        if (!duolingoDespawned)
        {
            if(timeToMove < 3)
            {
                timeToMove += Time.deltaTime;
            }
            else if(timeToMove >= 3)
            {
                duoNavMesh.destination = playerTransform.position;
                if(Vector3.Distance(destino, playerTransform.position) > 1.0f)
                {
                    duoAnimator.SetBool("walk", true);
                }
            }
        }
        if (duoGoSpawn)
        {
            DuolingoSpawn();
        }

        if (createNewSpawn)
        {
            duolingoActualSpawn = Random.Range(0, duolingoSpawns.Length - 1);
            while (duolingoActualSpawn == duolingoPreviusSpawn)
            {
                duolingoActualSpawn = Random.Range(0, duolingoSpawns.Length - 1);
            }
            duolingoPreviusSpawn = duolingoActualSpawn;
            createNewSpawn = false;
        }
    }
    private void DuolingoSpawn()
    {
        if(newDuolingoSpawn > transcurredTime && duolingoDespawned)
        {
            if (countdownUp)
            {
                transcurredTime += Time.deltaTime;
            }
        }
        else if(newDuolingoSpawn <= transcurredTime)
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
}
