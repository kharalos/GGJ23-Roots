using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeProfileManager : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    private ChromaticAberration _chromaticAberration;
    private bool _isChromaticAberrationNotNull;

    private void Start()
    {
        if (volumeProfile.TryGet(out _chromaticAberration))
        {
            _chromaticAberration.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Volume Profile Chromatic Aberration not found");
        }
    }
    public void ChangeChroma(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);
        _chromaticAberration.intensity.value = value;
    }
    public void AddChroma(float value)
    {
        _chromaticAberration.intensity.value += value;
        _chromaticAberration.intensity.value = Mathf.Clamp(_chromaticAberration.intensity.value, 0f, 1f);
    }

    private void OnDestroy()
    {
        ResetProfile();
    }

    private void ResetProfile()
    {
        if(_chromaticAberration != null)
        {
            _chromaticAberration.intensity.value = 0f;
        }
    }
}
