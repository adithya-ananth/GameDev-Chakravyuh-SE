using UnityEngine;

public class SubmarineMovement : MonoBehaviour
{
    public float verticalSpeed = 5f;
    public float upperLimit = 4.5f;
    public float lowerLimit = -4.5f;
    public GameObject gameOverPanel;
    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver)
        {
            float moveInput = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(0, moveInput * verticalSpeed * Time.deltaTime, 0);
            transform.position += move;

            float yPos = Mathf.Clamp(transform.position.y, lowerLimit, upperLimit);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // Optionally stop game physics by freezing time
        // Time.timeScale = 0;
    }
}
