using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float speed = 2f; // Base speed
    public float speedIncrement = 0.5f; // Speed increase on mouse click
    public GameObject oilSpillPrefab; // Assign the oil spill prefab here

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        // Move the ship to the right
        rb.linearVelocity = new Vector2(speed, 0); // Ensure movement along the x-axis

        // Increase speed when the mouse is clicked
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            speed += speedIncrement;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the ship collides with the background
        if (collision.GetComponent<PolygonCollider2D>() != null)
        {
            SpawnOilSpill();

            // Trigger smooth destruction instead of directly destroying the ship
            ShipCollisionHandler shipCollisionHandler = GetComponent<ShipCollisionHandler>();
            if (shipCollisionHandler != null)
            {
                StartCoroutine(shipCollisionHandler.SmoothDestroy());
            }
            else
            {
                Debug.LogWarning("ShipCollisionHandler not found! Destroying instantly.");
                Destroy(gameObject); // Fallback if the handler is not attached
            }
        }
    }

    void SpawnOilSpill()
    {
        // Create the oil spill at the ship's current position
        Instantiate(oilSpillPrefab, transform.position, Quaternion.identity);
        Debug.Log("Oil spill created at: " + transform.position);
    }
}
