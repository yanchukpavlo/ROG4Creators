using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Bullet : MonoBehaviour
{
    [SerializeField]
    private float timeToDestroy = 10f;
    
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    protected int damage = 40;

    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(gameObject, timeToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        var damageable = hitInfo.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Hit(damageable);
        }
    }

    public abstract void Hit(IDamageable hitInfo);
}