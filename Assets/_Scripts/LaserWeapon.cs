using System.Collections;
using UnityEngine;

public class LaserWeapon : Weapon
{
    [Header("Arm")]
    [SerializeField] [Range(0.3f, 1)]
    private float armIkRadius = 0.5f;
    [SerializeField]
    [Range(0.25f, 3)]
    private float ikYMult = 2;
    [SerializeField]
    [Range(0.25f, 3)]
    private float ikXMult = 0.5f;
    [SerializeField]
    private Transform armTransform;

    [Space]

    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private LayerMask collisionLayerMask;

    private Vector3 startArmPos;

    private void Awake()
    {
        startArmPos = armTransform.localPosition;
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetButton("Fire1") && timer <= 0)
            {
                timer = fireRate;
                Shoot();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = pos - armTransform.position;
        dir.Normalize();
 
        if (transform.eulerAngles.y > 90)
        {
            dir.x *= -1;
        }
        
        armTransform.localPosition = startArmPos + (new Vector3(dir.x * ikXMult, dir.y * ikYMult) * armIkRadius);
    }

    public override void Shoot()
    {
        StartCoroutine(Laser());
    }

    private IEnumerator Laser()
    {
        CameraShake.instance.Shake(cameraShakeAmount, cameraShakeLength);
        shootAudioSource.Play();
        var hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, 9999, collisionLayerMask);
        GameObject impactGameObject = null;
        if (hitInfo)
        {
            var enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            impactGameObject = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;

        yield return new WaitForSeconds(0.75f);
        if (impactGameObject != null)
        {
            Destroy(impactGameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(armTransform.position, armIkRadius);
    }
}