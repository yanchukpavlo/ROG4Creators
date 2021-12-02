using UnityEngine;

public class PrefabWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private GameObject bulletPrefab;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}