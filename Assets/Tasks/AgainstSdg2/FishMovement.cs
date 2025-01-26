using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 0.5f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trash") || collision.CompareTag("OilSpill"))
        {
            Destroy(gameObject); // Fish dies on collision
            Debug.Log("Fish killed by collision.");
        }
    }
}
