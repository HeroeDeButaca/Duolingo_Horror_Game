using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoScript : MonoBehaviour
{
    private Rigidbody rb;
    private DuoController duoController;
    [SerializeField] private GameObject prefabParticle;
    private Transform duo;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        duoController = GameObject.FindGameObjectWithTag("Duolingo").GetComponent<DuoController>();
        duo = GameObject.FindGameObjectWithTag("DuoCenter").GetComponent<Transform>();
    }
    void Update()
    {
        rb.AddForce(transform.forward * 30);
        Destroy(this.gameObject, 0.4f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Duolingo"))
        {
            Destroy(this.gameObject);
            Instantiate(prefabParticle, duo.position, Quaternion.identity);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
