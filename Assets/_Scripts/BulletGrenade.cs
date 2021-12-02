using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGrenade : Bullet
{
    bool active = true;

    public override void Hit(IDamageable hitInfo)
    {
        if (active) GetComponent<Animator>().SetTrigger("hit");
        active = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        hitInfo.TakeDamage(damage);
    }
}
