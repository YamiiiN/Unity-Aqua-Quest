using UnityEngine;
using UnityEngine.UIElements;

public class CharacterAnimation : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Joystick joystick;
    public BoxCollider2D boundary;
    public UnityEngine.UI.Button attackButton; // Assign in Inspector

    private Vector2 movement;
    private Vector3 originalScale;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private bool isAttacking = false;

    private BoxCollider2D seg1, seg2, seg3;

    GameListener gameListener;

    // Seg1 = gameListener.bound1;

    private void Awake() // Or use Start()
    {
        gameListener = FindFirstObjectByType<GameListener>(); // Find GameListener in the scene
        
        if (gameListener != null)
        {
            seg1 = gameListener.bound1; // Assign the BoxCollider2D
            seg2 = gameListener.bound2; // Assign the BoxCollider2D
            seg3 = gameListener.bound3; // Assign the BoxCollider2D
        }
        else
        {
            Debug.LogError("GameListener not found in the scene!");
        }
    }

    private void Start()
    {
        originalScale = transform.localScale;
        if (boundary != null)
        {
            minBounds = boundary.bounds.min;
            maxBounds = boundary.bounds.max;
        }
        else
        {
            Debug.LogError("Boundary BoxCollider2D is not assigned!");
        }

        // Attach Attack function to button click
        attackButton.onClick.AddListener(TriggerAttack);
    }

    private void Update()
    {
        if(seg1.isActiveAndEnabled)
        {
            minBounds = seg1.bounds.min;
            maxBounds = seg1.bounds.max;
        }
        else if(seg2.isActiveAndEnabled)
        {
            minBounds = seg2.bounds.min;
            maxBounds = seg2.bounds.max;
        }
        else if(seg3.isActiveAndEnabled)
        {
            minBounds = seg3.bounds.min;
            maxBounds = seg3.bounds.max;
        }
        if (isAttacking) return; // Prevent movement during attack

        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;

        bool isMoving = movement.sqrMagnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // Flip character based on direction
            if (movement.x > 0.1f)
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else if (movement.x < -0.1f)
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking) return; // Prevent movement during attack

        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        rb.MovePosition(newPosition);
    }

    private void TriggerAttack()
    {
        if (isAttacking) return; // Prevent spamming attack
        isAttacking = true;

        animator.SetTrigger("Attack");
        Invoke(nameof(ResetAttack), 0.5f); // Adjust time to match attack animation duration
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}
