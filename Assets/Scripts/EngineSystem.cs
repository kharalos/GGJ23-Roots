using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] jetFlames;
    [SerializeField] private float lowEmissionRate = 5f;
    [SerializeField] private float highEmissionRate = 15f;
    [SerializeField] private float lowPitch = 1f;
    [SerializeField] private float highPitch = 2f;
    [SerializeField] private float volumeClamp = 0.2f;
    [SerializeField] private AudioSource audioSource;
    
    public void SetFuelEmission(float input, bool boost)
    {
        foreach (var flame in jetFlames)
        {
            var emissionModule = flame.emission;
            emissionModule.rateOverTime = input * (boost ? highEmissionRate : lowEmissionRate);
            audioSource.volume = Mathf.Clamp(input, 0f, volumeClamp);
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, boost ? highPitch : lowPitch, Time.deltaTime * 3f);
        }
    }
}
