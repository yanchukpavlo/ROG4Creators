using UnityEngine;

public class EnemyJumpPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (enemy.CanJump) enemy.Jump();
        }
    }
}
