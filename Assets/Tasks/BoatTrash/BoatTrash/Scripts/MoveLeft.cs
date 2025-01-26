using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 5f;
    public bool isGameOver = false;

    private void Update()
    {
        if (!isGameOver)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (transform.position.x < -10f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Submarine"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;

        // No longer activating the gameOverPanel, just setting gameOver flag.
    }
}
