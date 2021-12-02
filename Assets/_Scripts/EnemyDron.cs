using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDron : Enemy
{
    [Space]
    [SerializeField] bool range;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] Transform rangePoint;
    [SerializeField] float waitBetweeAttack = 4f;
    [SerializeField] float distanseForAttack = 3f;
    [SerializeField] GameObject bulletPefab;

    Vector3 moveDirection;
    bool inMeleeAttack;

    public override void InStart()
    {
        canJump = false;

        StartCoroutine(ChengeDirection());
    }

    public override void Check()
    {
        if (canAttack && alive)
        {
            if (Physics2D.OverlapCircle(rangePoint.position, distanseForAttack, attackLayer) != null)
            {
                Attack(!range);
                return;
            }
        }
    }

    public override void Attack(bool isMelee)
    {
        move = false;
        canAttack = false;
        if (isMelee)
        {
            animator.SetTrigger(AnimatorMelee);
        }
        else animator.SetTrigger(AnimatorRange);
        StartCoroutine(WaitAndDo(waitBetweeAttack, delegate { move = true; canAttack = true; }));
    }

    public override void Move()
    {
        if (move && alive)
        {
            transform.Translate(moveDirection * Time.deltaTime * moveSpeed, Space.World);
            animator.SetFloat(AnimatorSpeed, 1);
        }
        else
        {
            animator.SetFloat(AnimatorSpeed, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inMeleeAttack)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                inMeleeAttack = false;
                player.TakeDamage(meleeDamage);
            }
        }
    }

    void RangeAttack()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction = direction.normalized;
        Quaternion rot = Quaternion.FromToRotation(transform.right, direction) * transform.rotation;

        GameObject bullet = Instantiate(bulletPefab, rangePoint.position, rot);
    }

    void MeleeAttack()
    {
        inMeleeAttack = true;
        StartCoroutine(WaitAndDo(0.2f, delegate { inMeleeAttack = false;}));
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rangePoint.position, distanseForAttack);
        }
    }

    IEnumerator ChengeDirection()
    {
        moveDirection = player.transform.position - transform.position;
        moveDirection = moveDirection.normalized;
        yield return new WaitForSeconds(2);
        StartCoroutine(ChengeDirection());
    }
}
