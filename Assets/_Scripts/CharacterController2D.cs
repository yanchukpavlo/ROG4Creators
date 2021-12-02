using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour, IDamageable
{
    public static CharacterController2D instance;

    [SerializeField] [Min(1)]
    private int health = 500;
    [SerializeField] [Range(0, .3f)] 
    private float movementSmoothing = .05f; // How much to smooth out the movement

    [SerializeField] [Min(1)] 
    private float runSpeed = 15f;
    [SerializeField] [Min(1)]
    private float jumpForce = 7f;
    [SerializeField] [Min(0.1f)]
    private float timeBetweenJump = 1f;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private RuntimeAnimatorController animatorControllerDie;

    private Animator animator;
    private Rigidbody2D rb2D;
    
    private float horizontalMove;
    private float timerToJump;
    private bool alive;
    private bool inGame = true;
    private bool facingRight = true; // For determining which way the player is currently facing.
    private bool grounded;
    private Vector3 velocity = Vector3.zero;
    private LaserWeapon weapon;
    
    private static readonly int AnimatorSpeed = Animator.StringToHash("speed");
    private static readonly int AnimatorGrounded = Animator.StringToHash("grounded");
    private static readonly int AnimatorJump = Animator.StringToHash("jump");
    private static readonly int AnimatorHit = Animator.StringToHash("hit");

    private void Awake()
    {
        instance = this;

        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<LaserWeapon>();

        InvokeRepeating("CheckIsGrounded", 0.1f, 0.1f);
    }

    private void Start()
    {
        EventsManager.instance.onGameStart += Instance_onGameStart;
    }

    private void OnDestroy()
    {
        EventsManager.instance.onGameStart -= Instance_onGameStart;
    }

    private void Update () {

        if (alive)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (timerToJump <= 0 && grounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    timerToJump = timeBetweenJump;
                    animator.SetTrigger(AnimatorJump);
                }
            }
            else
            {
                timerToJump -= Time.deltaTime;
            }

            animator.SetFloat(AnimatorSpeed, Mathf.Abs(horizontalMove));
        }

        if (inGame) animator.SetBool(AnimatorGrounded, grounded);
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            Move(horizontalMove * Time.fixedDeltaTime);
        }
    }

    private void Instance_onGameStart(bool state)
    {
        alive = state;
        inGame = state;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UIManager.instance.UpdatePlayerHP(-damage);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger(AnimatorHit);
        }
    }

    private void Die()
    {
        EventsManager.instance.GameStart(false);
        GetComponent<LaserWeapon>().enabled = false;
        animator.runtimeAnimatorController = animatorControllerDie;
        UIManager.instance.UpdateData();
    }

    private void Jump()
    {
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Move(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rb2D.velocity.y);
        // And then smoothing it out and applying it to the character
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void CheckIsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.01f, groundLayer) != null;
    }
}