using UnityEngine;

public class NoPassar : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().GoSpawn = true;
        }
    }
}
