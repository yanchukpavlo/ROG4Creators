using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected bool showGizmos;

    [SerializeField]
    int score = 10;

    [SerializeField]
    protected int health = 100;

    [SerializeField]
    protected float moveSpeed = 10f;
    
    [SerializeField]
    protected float jumpForce = 10f;

    [SerializeField]
    protected float yOffsrt = 0.6f;

    [SerializeField] 
    protected int meleeDamage = 40;


    protected bool alive = true;
    protected bool move = true;
    protected bool canAttack = true;
    protected bool canJump;
    protected bool facingRight = true;
    protected CharacterController2D player;
    protected Rigidbody2D rb;
    protected Animator animator;

    public bool CanJump { get { return canJump; } }

    protected static readonly int AnimatorMelee = Animator.StringToHash("melee");
    protected static readonly int AnimatorRange = Animator.StringToHash("range");
    protected static readonly int AnimatorSpeed = Animator.StringToHash("speed");

    public abstract void InStart();

    public abstract void Check();

    public abstract void Move();

    public abstract void Attack(bool isMelee);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = CharacterController2D.instance;
        InvokeRepeating("Flip", 0, UnityEngine.Random.Range(1f, 2f));

        InStart();
    }

    private void Update()
    {
        Check();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Jump()
    {
        if (alive)
        {
            if (player.transform.position.y > transform.position.y + yOffsrt)
            {
                animator.SetTrigger("jump");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (alive)
        {
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Die()
    {
        alive = false;
        animator.SetTrigger("die");
        rb.velocity = new Vector2(0, rb.velocity.y);
        WaveManager.instance.EnemyDead();
        UIManager.instance.AddScore(score);
    }

    private void Flip()
    {
        facingRight = player.transform.position.x > transform.position.x;
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void AddForceForJump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    protected IEnumerator WaitAndDo(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    } 
}