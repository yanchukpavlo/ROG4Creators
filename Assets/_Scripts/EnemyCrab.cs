using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrab : Enemy
{
    [Space]
    [SerializeField] bool range;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] Transform meleePoint;
    [SerializeField] Transform rangePoint;
    [SerializeField] float waitBetweeAttack = 2f;
    [SerializeField] float meleeAttackRadius = 0.9f;
    [SerializeField] float rangeAttackDistanse = 3f;
    [SerializeField] GameObject bulletPefab;

    public override void InStart()
    {
        canJump = true;
    }

    public override void Check()
    {
        if (canAttack && alive)
        {
            if (Physics2D.OverlapCircle(meleePoint.position, meleeAttackRadius * 0.75f, attackLayer) != null)
            {
                Attack(true);
                return;
            }

            if (range)
            {
                if (Physics2D.Raycast(rangePoint.position, rangePoint.right, rangeAttackDistanse, attackLayer))
                {
                    Attack(false);
                }
            }
        }
    }

    public override void Attack(bool isMelee)
    {
        move = false;
        canAttack = false;
        if (isMelee) animator.SetTrigger(AnimatorMelee);
        else animator.SetTrigger(AnimatorRange);
        StartCoroutine(WaitAndDo(waitBetweeAttack, delegate { move = true; canAttack = true; }));
    }

    public override void Move()
    {
        if (alive)
        {
            if (move)
            {
                if (facingRight)
                {
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                }

                animator.SetFloat(AnimatorSpeed, 1);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetFloat(AnimatorSpeed, 0);
            }
        }
    }

    void MeleeAttack()
    {
        if (Physics2D.OverlapCircle(meleePoint.position, meleeAttackRadius, attackLayer) != null)
        {
            CharacterController2D.instance.TakeDamage(meleeDamage);
        }
    }
    
    void RangeAttack()
    {
        GameObject bullet = Instantiate(bulletPefab, rangePoint.position, transform.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleePoint.position, meleeAttackRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(meleePoint.position, meleeAttackRadius * 0.75f);

            Debug.DrawRay(rangePoint.position, rangePoint.right * rangeAttackDistanse, Color.yellow);
        }
    }
}
