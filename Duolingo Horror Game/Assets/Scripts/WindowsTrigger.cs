using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindowsTrigger : MonoBehaviour
{
    private bool climb, startClimb, navMeshReactivate;
    [SerializeField] private Transform[] Moves;
    [SerializeField] private Transform duoTransform;
    [SerializeField] private GameObject duoGO;
    private int i = 0;
    [SerializeField] private float speed = 1.0f, timeTranscurred;
    private DuoController duoController;
    void Awake()
    {
        duoController = GameObject.FindGameObjectWithTag("Duolingo").GetComponent<DuoController>();
        startClimb = false;
        climb = false;
        timeTranscurred = 0;
    }
    void Update()
    {
        if (startClimb)
        {
            StartClimb();
        }

        if (climb)
        {
            Climb_window();
        }
        if (navMeshReactivate)
        {
            if (timeTranscurred < 0.25f)
            {
                timeTranscurred += Time.deltaTime;
            }
            else if (timeTranscurred >= 0.25f)
            {
                timeTranscurred = 0;
                navMeshReactivate = false;
                duoGO.GetComponent<NavMeshAgent>().enabled = true;
                duoGO.GetComponent<DuoController>().isClimbing = false;
                duoGO.GetComponent<Rigidbody>().mass = 1000;
                duoGO.GetComponent<BoxCollider>().enabled = true;
                duoGO.GetComponent<NavMeshAgent>().destination = duoTransform.position; // ¿porque puse esto?
                duoGO.GetComponent<DuoController>().isInside = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Duolingo"))
        {
            startClimb = true;
        }
    }
    private void Climb_window()
    {
        if(duoTransform.position != Moves[i].position && duoTransform.position != duoController.duoHidedTransform.position)
        {
            duoTransform.position = Vector3.MoveTowards(duoTransform.position, Moves[i].position, Time.deltaTime * speed);
        }
        else if(duoTransform.position == Moves[i].position && i < Moves.Length - 1 && duoTransform.position != duoController.duoHidedTransform.position)
        {
            i++;
            speed += 0.2f;
        }
        if(duoTransform.position != Moves[i].position && duoTransform.position == duoController.duoHidedTransform.position)
        {
            duoGO.GetComponent<Animator>().SetBool("climb_window", false);
            duoGO.GetComponent<Animator>().SetBool("walk", true);
            duoGO.GetComponent<Rigidbody>().useGravity = true;
            climb = false;
            i = 0;
            speed = 1.0f;
            navMeshReactivate = false;
            duoGO.GetComponent<NavMeshAgent>().enabled = true;
            duoGO.GetComponent<DuoController>().isClimbing = false;
            duoGO.GetComponent<Rigidbody>().mass = 1000;
            duoGO.GetComponent<BoxCollider>().enabled = true;
            duoGO.GetComponent<NavMeshAgent>().destination = duoTransform.position;
            duoGO.GetComponent<DuoController>().isInside = true;
        }

        if(i == Moves.Length - 1 && duoTransform.position == Moves[i].position && duoTransform.position != duoController.duoHidedTransform.position)
        {
            duoGO.GetComponent<Animator>().SetBool("climb_window", false);
            duoGO.GetComponent<Animator>().SetBool("walk", true);
            duoGO.GetComponent<Rigidbody>().useGravity = true;
            climb = false;
            navMeshReactivate = true;
            i = 0;
            speed = 1.0f;
        }
    }
    private void StartClimb()
    {
        duoGO.GetComponent<DuoController>().isClimbing = true;
        duoGO.GetComponent<Animator>().SetBool("climb_window", true);
        duoGO.GetComponent<Animator>().SetBool("walk", false);
        duoGO.GetComponent<Animator>().SetBool("run", false);
        duoGO.GetComponent<NavMeshAgent>().enabled = false;
        duoGO.GetComponent<BoxCollider>().enabled = false;
        duoGO.GetComponent<Rigidbody>().useGravity = false;
        duoGO.GetComponent<Rigidbody>().mass = 1;
        climb = true;
        startClimb = false;
    }
}
