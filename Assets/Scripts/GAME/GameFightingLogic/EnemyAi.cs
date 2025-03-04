using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 3f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private Vector3 originalScale;
    private GameObject playerObj;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform; // Store the initial scale for flipping
    }

    void Update()
    {
        if (!playerObj.activeSelf){
            anim.SetTrigger("YIPIE");
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // If currently attacking but the player moves out of range, stop attacking
        if (isAttacking && distance > attackRange)
        {
            StopAttacking();
            return;
        }

        if (!isAttacking)
        {
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
            else if (distance > attackRange)
            {
                ChasePlayer();
            }
        }
    }

    void ChasePlayer()
    {
       
            if(!playerObj.activeSelf)
            {
                 anim.SetTrigger("YIPIE");
                return;
            }
            anim.SetBool("IsPlayerFar", true);
            Vector2 direction = (player.position - transform.position).normalized;

            // Move the enemy smoothly
            rb.linearVelocity = direction * moveSpeed;

            // Flip only on X-axis (don't rotate or lay down)
            if (direction.x < 0.1f)
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else if (direction.x > -0.1f)
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        

    }

    void Attack()
    {
        if(!playerObj.activeSelf)
            {
                 anim.SetTrigger("YIPIE");
                return;
            }
        isAttacking = true;
        anim.SetBool("IsPlayerFar", false);
        
        rb.linearVelocity = Vector2.zero; // Stop movement when attacking
        lastAttackTime = Time.time;
    }

    void StopAttacking()
    {
        isAttacking = false;
        anim.SetBool("IsPlayerFar", true);
    }

  
    public void DealDamage(Collider2D other)
    {
        // Check if the collider belongs to the player
        
            // Get the PlayerHealth component and deal damage
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Player hit! Damage dealt: " + attackDamage);
            }
        
    }



    public void FinishAttack()
    {
        isAttacking = false;
    }
}
