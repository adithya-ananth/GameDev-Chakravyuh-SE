using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;             // Movement speed of the player (increased from 6f to 10f)
    public float collisionOffset = 0.05f;     // Offset used to check for potential collisions
    public ContactFilter2D movementFilter;    // Filter to specify what type of colliders we want to detect

    private float horizontalInput;            // Input for horizontal movement (X axis)
    private float verticalInput;              // Input for vertical movement (Y axis)
    private Vector2 movementInput;            // Movement input from the player (combined horizontal and vertical)
    private Rigidbody2D rb;                   // Reference to Rigidbody2D component

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); // List to store detected collisions

    private Animator animator;                // Reference to Animator to trigger animations

    PhotonView view;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            // Get input from the keyboard (or gamepad) for horizontal and vertical directions
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Combine the input into a Vector2 for movement
            movementInput = new Vector2(horizontalInput, verticalInput);

            // If there is movement input, try to move the player
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                if (!success)
                {
                    // Try moving in X direction first
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                    {
                        // If X movement fails, try moving in Y direction
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }

                // Handle animation based on movement input
                animator.SetFloat("Xinput", movementInput.x);
                animator.SetFloat("Yinput", movementInput.y);
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        // Cast in the direction of movement to check for collisions
        int count = rb.Cast(
            direction,                // Direction to move in
            movementFilter,           // The filter specifying what colliders we want to detect
            castCollisions,           // List to store detected collisions
            moveSpeed * Time.deltaTime + collisionOffset); // The distance to move per frame

        // If no collisions detected, move the player
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
            return true;
        }

        return false;
    }
}
