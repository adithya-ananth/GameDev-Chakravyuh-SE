using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed
    private Rigidbody2D rb; // Rigidbody2D component for physics-based movement
    private Vector2 movement; // Vector to store movement input
    public Vector2 minBounds = new Vector2(-10f, -5f); // Minimum x and y boundaries (default values)
    public Vector2 maxBounds = new Vector2(10f, 5f); // Maximum x and y boundaries (default values)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component attached to the GameObject
    }

    void Update()
    {
        // Get input from horizontal and vertical axes (arrow keys, WASD, or joystick)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Clamp the position to keep it within bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        rb.MovePosition(newPosition);
    }
}
