using UnityEngine;

public class Parpadeo_anim : MonoBehaviour
{
    [SerializeField] private Animator[] animators;
    void Start()
    {
        animators[0].SetBool("arr_izq", true);
        animators[1].SetBool("arr_der", true);
        animators[2].SetBool("aba_izq", true);
        animators[3].SetBool("aba_der", true);
    }
}
