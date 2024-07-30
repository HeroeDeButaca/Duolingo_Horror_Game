using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-- AudioSource --")]
    [SerializeField] private AudioSource audioBGM;
    [SerializeField] private AudioSource audioSFX;
    public AudioSource audioSteps;
    public AudioSource audioStepsDuo;
    [SerializeField] private AudioSource audioJumpscare;

    [Header("-- AudioClips --")]
    public AudioClip jumpscare;
    public AudioClip pasosMadera;
    public AudioClip correctoLesson;
    public AudioClip incorrectoLesson;
    public AudioClip lessonComplete;
    public AudioClip ataqueLanzado;
    public AudioClip recargaLinterna;
    public AudioClip infrasonido;
    public AudioClip abrirPuerta;
    public AudioClip cerrarPuerta;
    void Start()
    {
        audioBGM.clip = infrasonido;
        audioBGM.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        audioSFX.PlayOneShot(clip);
    }
    public void PlaySteps(AudioClip clip)
    {
        audioSteps.PlayOneShot(clip);
    }
    public void PlayStepsDuo(AudioClip clip)
    {
        audioStepsDuo.PlayOneShot(clip);
    }
    public void PlayJumpscare()
    {
        audioJumpscare.PlayOneShot(jumpscare);
    }
}
