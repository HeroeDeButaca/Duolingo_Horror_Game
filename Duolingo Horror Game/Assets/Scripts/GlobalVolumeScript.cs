using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeScript : MonoBehaviour
{
    [SerializeField] private VolumeProfile volume;
    [SerializeField] private Vignette vignette;
    private bool cambVignette, pasaVignette;
    private float t1, intensityFloat;
    [SerializeField] private ClampedFloatParameter intensity;
    void Start()
    {
        for (int i = 0; i < volume.components.Count; i++)
        {
            if (volume.components[i].name == "Vignette")
            {
                vignette = (Vignette)volume.components[i];
            }
        }
        intensity = vignette.intensity;
        intensity.value = 0.436f;
    }
    void Update()
    {
        if (pasaVignette)
        {
            vignetteVoid();
            intensity.value = intensityFloat;
        }
    }
    public void cambiarVignette(bool aumentar)
    {
        t1 = 0;
        cambVignette = aumentar;
        pasaVignette = true;
    }
    private void vignetteVoid()
    {
        if (cambVignette && t1 <= 1)
        {
            intensityFloat = Mathf.Lerp(0.436f, 0.631f, t1);
            t1 +=  2f * Time.deltaTime;
        }
        else if (!cambVignette && t1 < 1)
        {
            intensityFloat = Mathf.Lerp(0.631f, 0.436f, t1);
            t1 += 2f * Time.deltaTime;
        }
        if (t1 >= 1)
        {
            pasaVignette = false;
            if (cambVignette)
            {
                intensityFloat = 0.631f;
            }
            else if (!cambVignette)
            {
                intensityFloat = 0.436f;
            }
        }
    }
}
