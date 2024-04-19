using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puertas : MonoBehaviour
{
    public bool doorOpen;
    public float doorOpenAngle = 95, doorCloseAngle = 0, smooth = 3;
    Quaternion targetRotation, targetRotation2;

    public void ChangeDoorState()
    {
        doorOpen = !doorOpen;
    }
    void Update()
    {
        if (doorOpen)
        {
            targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else
        {
            targetRotation2 = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
        }
    }
}
