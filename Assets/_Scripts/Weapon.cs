using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    protected int damage = 40;

    [SerializeField]
    protected float fireRate = 1f;

    [SerializeField]
    protected float cameraShakeAmount = 0.05f;

    [SerializeField]
    protected float cameraShakeLength = 0.1f;

    [SerializeField]
    protected AudioSource shootAudioSource;

    [Space]

    protected bool active;
    protected float timer;

    public abstract void Shoot();

    private void Start()
    {
        EventsManager.instance.onGameStart += Instance_onGameStart;
    }

    private void OnDestroy()
    {
        EventsManager.instance.onGameStart -= Instance_onGameStart;
    }

    private void Instance_onGameStart(bool state)
    {
        active = state;
    }
}
