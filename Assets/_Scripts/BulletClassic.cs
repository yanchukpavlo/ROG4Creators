using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClassic : Bullet
{
    [Space]
    [SerializeField]
    private GameObject impactEffect;

    public override void Hit(IDamageable hitInfo)
    {
        hitInfo.TakeDamage(damage);
        StartCoroutine(SpawnImpactEffect());
    }

    private IEnumerator SpawnImpactEffect()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        rb.velocity = Vector2.zero;
        var impactGameObject = Instantiate(impactEffect, transform.position, transform.rotation);

        yield return new WaitForSeconds(1f);
        if (impactGameObject != null)
        {
            Destroy(impactGameObject);
        }

        Destroy(gameObject);
    }


}
