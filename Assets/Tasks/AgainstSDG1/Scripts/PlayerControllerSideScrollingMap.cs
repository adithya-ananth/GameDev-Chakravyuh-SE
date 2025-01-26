using System.Collections;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;      
    [SerializeField] private static float jumpForce = 3f;
    [SerializeField] private TextMeshProUGUI timerText;

    private Rigidbody2D body;
    private bool isGrounded;
    public bool gameOver;

    private bool isTimerRunning = false;            // Flag to check if the timer is running
    private float remainingTime = 120f;

    private int logs;

    AudioManager audioManager;

    private void Start()
    {
        audioManager.PlaySFX(audioManager.gameStart);
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        isGrounded = true;
        gameOver = false;
    }

    public static void modifyJumpForce()
    {
        jumpForce-= WeightController.logs * 0.1f;    
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            body.position = new Vector2(11.65f, 0.12f);
        }

        if (gameOver)
        {
            timerText.text = "Task Failed";
            audioManager.PlaySFX(audioManager.gameOver);
            SceneManager.LoadScene("Map2");
            return;
        }
        // Horizontal movement
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            audioManager.PlaySFX(audioManager.jump);
        }

        if (isTimerRunning && remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime; // Decrease the remaining time

            // Format time as minutes:seconds
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Display formatted time
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
        else if (remainingTime <= 0f)
        {
            // Stop the timer if time runs out
            isTimerRunning = false;
            remainingTime = 0f; // Prevent negative time
            timerText.text = "Time: 00:00"; // Display final time
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Path"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("ground"))
        {
            gameOver = true;
            audioManager.PlaySFX(audioManager.gameOver);
            Debug.Log("Fell on the ground");
            SceneManager.LoadScene("Map2");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player leaves the ground
        if (collision.gameObject.CompareTag("Path"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Start or stop the timer when the player enters a trigger with the "timerTrigger" tag
        if (other.CompareTag("timerTrigger"))
        {
            if (!isTimerRunning) // Start timer if it's not running
            {
                isTimerRunning = true;
                remainingTime = 120f; // Reset to 2 minutes
            }
            else // Stop timer if it's already running
            {
                isTimerRunning = false;
                if(remainingTime > 0f)
                {
                    timerText.text = "River Polluted Successfully";
                    Debug.Log("Finished task");
                    audioManager.PlaySFX(audioManager.gameOver);
                    //SceneManager.LoadScene("Map");

                    StartCoroutine(FailureWithDelay());

                }
                else
                {
                    gameOver = true;
                    audioManager.PlaySFX(audioManager.gameOver);
                    Debug.Log("Failed");
                    //SceneManager.LoadScene("Map");

                    PhotonNetwork.LoadLevel("Map3");
                }
            }
        }
    }

    private IEnumerator FailureWithDelay()
    {
        yield return new WaitForSeconds(3f);

        // Deduct points
        PointsManager.IncrementPoints(-20-(WeightController.logs)*3);

        // Load the next map
        PhotonNetwork.LoadLevel("Map3");
    }
}
