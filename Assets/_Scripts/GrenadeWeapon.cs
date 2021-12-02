using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeapon : Weapon
{
    [SerializeField] GameObject bulletPref;
    [SerializeField] AudioSource reloadSource;

    bool canShot = true;

    private void Update()
    {
        if (active && canShot)
        {
            if (Input.GetButton("Fire2"))
            {
                Shoot();
            }
        }
    }

    public override void Shoot()
    {
        canShot = false;
        shootAudioSource.Play();
        var bullet = Instantiate(bulletPref, firePoint.position, firePoint.rotation);
        timer = 0;
        StartCoroutine(Reload(1/fireRate));
    }

    IEnumerator Reload(float t)
    {
        UIManager.instance.SetSecondWeaponSlider(timer/fireRate);
        yield return new WaitForSeconds(t);
        timer += t;
        if (timer >= fireRate)
        {
            canShot = true;
            UIManager.instance.SetSecondWeaponSlider(1);
            reloadSource.Play();
        }
        else
        {
            StartCoroutine(Reload(t));
        }
    }
}
