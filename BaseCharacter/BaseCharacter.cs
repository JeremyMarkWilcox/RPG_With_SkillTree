using System.Collections;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public BaseCharacterFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public BaseCharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion
    #region Attack
    [Header("Attack info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    #endregion
    #region Knockback
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    protected bool isKnockedback;
    [SerializeField] protected float KnockedbackDuration;
    #endregion
    #region Collisions
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    #endregion
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        fx = GetComponent<BaseCharacterFX>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<BaseCharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }
    
    protected virtual void Update()
    {
        
    }

    public virtual void SlowBaseCharacter(float _slowPercentage, float _slowDuration)
    {
        
    }

    protected virtual void ReturnDefaultSpeed() 
    { 
        anim.speed = 1;
    }

    public virtual void DamageImpact() => StartCoroutine("HitKnockback");
   
    protected virtual IEnumerator HitKnockback()
    {
        isKnockedback = true;

        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(KnockedbackDuration);
        isKnockedback = false;
    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    #region Velocity
    public virtual void SetZeroVelocity()
    {
        if (isKnockedback)
            return;
        rb.linearVelocity = new Vector2(0, 0);
    }
        

    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnockedback)
            return;

        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFlipped != null)
            onFlipped();
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion

    

    public virtual void Die()
    {

    }
}
