using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlienAttack : MonoBehaviour
{
    [SerializeField] private float timeToSpawn, transcurredTime, alienAttTime;
    public float minimumTimeSpawn, maxTimeSpawn;
    public bool gameStart = false;
    private bool prepareSpawn, spawned;
    private Animator animator;
    [SerializeField] private GameObject explosionPrefab, prefabCreado;
    [SerializeField] private Material neon;
    private Color normalNeon, redNeon, lerpedColor, notVeryRed;
    [SerializeField] private TMP_Text explicacion;
    private GameOver gameOver;
    void Start()
    {
        normalNeon = new Vector4(1.72079539f, 1.68170476f, 0.98202455f, 1);
        redNeon = new Vector4(1.69356871f, 0.111839205f, 0.14862363f, 1);
        neon.SetColor("_EmissionColor", normalNeon);
        notVeryRed = new Vector4(1.87945795f, 0.563837171f, 0.59340173f, 1);
        animator = GetComponent<Animator>();
        gameOver = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameOver>();
        transcurredTime = 0;
        prepareSpawn = true;
    }

    void Update()
    {
        if(gameStart && prepareSpawn && !spawned)
        {
            timeToSpawn = Random.Range(minimumTimeSpawn, maxTimeSpawn);
            prepareSpawn = false;
        }
        else if(gameStart && !prepareSpawn && !spawned && transcurredTime < timeToSpawn)
        {
            transcurredTime += Time.deltaTime;
        }
        else if(gameStart && !prepareSpawn && !spawned && transcurredTime >= timeToSpawn)
        {
            animator.SetBool("Aparecer", true);
            spawned = true;
        }
        if(gameStart && spawned && alienAttTime < 35)
        {
            alienAttTime += Time.deltaTime;
            lerpedColor = Color.Lerp(notVeryRed, redNeon, Mathf.PingPong(Time.time, 1));
            neon.SetColor("_EmissionColor", lerpedColor);
            Debug.Log("Te atacan los aliens, " + lerpedColor);
        }
        else if(gameStart && spawned && alienAttTime >= 35)
        {
            gameOver.PartyEndAliens();
        }
        if(animator.GetBool("Explosion") && prefabCreado.GetComponent<ParticleSystem>().time >= 0.99f)
        {
            Destroy(prefabCreado);
            neon.SetColor("_EmissionColor", normalNeon);
            animator.SetBool("Aparecer", false);
            animator.SetBool("Explosion", true);
            alienAttTime = 0;
            prepareSpawn = true;
            spawned = false;
        }
    }
    public void AlienButton()
    {
        if (spawned)
        {
            animator.SetBool("Explosion", true);
            prefabCreado = Instantiate(explosionPrefab, this.gameObject.transform);
            transcurredTime = 0;
        }
        animator.SetBool("Pulsar", false);
    }
}
