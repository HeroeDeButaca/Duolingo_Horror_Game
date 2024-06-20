using UnityEngine;

public class PuertaTrigger : MonoBehaviour
{
    [SerializeField] private Puertas puerta;
    [SerializeField] private bool puertaVNoInteractable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Duolingo"))
        {
            puerta.doorOpen = true;
            if (puertaVNoInteractable)
            {
                puerta.isInteractable = false;
                puerta.doorOpen = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Duolingo") && puertaVNoInteractable)
        {
            puerta.isInteractable = false;
            puerta.doorOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Duolingo"))
        {
            if (!puertaVNoInteractable)
            {
                puerta.doorOpen = false;
            }
            puerta.isInteractable = true;
        }
    }
}
