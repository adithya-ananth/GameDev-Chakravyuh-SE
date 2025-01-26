using UnityEngine;

public class TrashBehavior : MonoBehaviour
{
    public float driftSpeed = 0.5f;

    void Update()
    {
        transform.Translate(Vector2.left * driftSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            Destroy(collision.gameObject); // Remove fish on collision
            Debug.Log("Fish destroyed by trash.");
        }
    }
}
