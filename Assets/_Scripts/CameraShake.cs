using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour {
    public static CameraShake instance;


    private CinemachineVirtualCamera mainCam;
    private CinemachineBasicMultiChannelPerlin camNoise;
    private float shakeAmount = 0;

    private void Awake()
    {
        instance = this;
        mainCam = GetComponent<CinemachineVirtualCamera>();
        camNoise = mainCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating(nameof(BeginShake), 0, 0.01f);
        Invoke(nameof(StopShake), length);
    }

    private void BeginShake()
    {
        if (!(shakeAmount > 0))
        {
            return;
        }

        camNoise.m_AmplitudeGain = Random.value * shakeAmount * 2 - shakeAmount;
    }

    private void StopShake()
    {
        CancelInvoke(nameof(BeginShake));
        camNoise.m_AmplitudeGain = 0;
    }
}
