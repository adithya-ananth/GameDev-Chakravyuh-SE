using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MainTimer : MonoBehaviourPunCallbacks
{
    [Header("Timer Settings")]
    public double timerDuration = 300.0;    // 5 minutes in seconds
    public float delayBeforeStart = 2f;    // 10 seconds delay before the timer starts
    public TMP_Text timerText;

    private double startTime = 0;           // The time at which the countdown starts
    private bool timerStarted = false;      // Flag to track if the timer has started
    private bool jobsAssigned = false;      // Flag to ensure jobs are assigned only once

    private const string TimerStartTimeKey = "StartTime";  // Key for room property

    private void Start()
    {
        // Always check if "StartTime" exists in room properties
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(TimerStartTimeKey))
        {
            startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties[TimerStartTimeKey];
            timerStarted = true; // Ensure the timer is flagged as started
            Debug.Log($"Timer resumed. StartTime: {startTime}");
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // Master client initializes the timer if it doesn't exist
            Debug.Log("Initializing timer...");
            InitializeTimer();
        }
    }

    /// <summary>
    /// Initialize the timer and store it in room properties.
    /// This is called only by the master client.
    /// </summary>
    private void InitializeTimer()
    {
        startTime = PhotonNetwork.Time + delayBeforeStart; // Set start time with a delay
        timerStarted = true;

        // Store the start time in room properties
        var roomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { TimerStartTimeKey, startTime }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        Debug.Log($"Timer initialized. StartTime: {startTime}");
    }

    private void Update()
    {
       
        if (!timerStarted)
        {
            // Show "Waiting to start" if the timer hasn't started yet
            timerText.text = "Waiting to start...";
            return;
        }

        // Calculate elapsed time and remaining time
        double elapsedTime = PhotonNetwork.Time - startTime;
        double remainingTime = timerDuration - elapsedTime;

        int currentPoints = (int)PhotonNetwork.CurrentRoom.CustomProperties["Points"];
        
        if(elapsedTime >= 299 || currentPoints == 100)
        {
            if(currentPoints >= 100)
            {
                timerText.text = "Collaborators Won!";
            }
            else
            {
                timerText.text = "Saboteurs Won!";
            }
        }

        if (remainingTime > 0)
        {
            if (elapsedTime <= 10)
            {
                timerText.text = "Waiting to assign...";
            }
            else
            {
                // Display the countdown timer
                int minutes = Mathf.FloorToInt((float)(remainingTime / 60));
                int seconds = Mathf.FloorToInt((float)(remainingTime % 60));
                timerText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
            }

            // Assign jobs if 10 seconds have passed
            if (elapsedTime >= 10 && !jobsAssigned)
            {
                AssignJobs();
                jobsAssigned = true; // Ensure jobs are assigned only once
            }
        }
        else
        {
            // Time's up
            timerText.text = "Time's Up!";
        }
    }

    /// <summary>
    /// Assign jobs to all players in the room.
    /// This is called only by the master client after 10 seconds.
    /// </summary>
    private void AssignJobs()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Debug.Log("Assigning jobs to all players...");

        // Check if the main player (master client) already has a job
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Job"))
        {
            Debug.Log("Jobs are already assigned. Skipping assignment.");
            return;
        }

        // Randomly choose one player to be the Saboteur
        Player[] players = PhotonNetwork.PlayerList;
        int saboteurIndex = Random.Range(0, players.Length);

        for (int i = 0; i < players.Length; i++)
        {
            // Assign "Saboteur" to the randomly chosen player
            string job = (i == saboteurIndex) ? "Saboteur" : "Collaborator";

            // Set the player's custom properties with their job
            var playerProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Job", job }
        };
            players[i].SetCustomProperties(playerProperties);

            Debug.Log($"Assigned {job} to {players[i].NickName}");
        }
    }

    public static void RemoveLocalPlayerPrefab()
    {
        // Find the local player's GameObject by their nickname
        string localPlayerNickname = PhotonNetwork.LocalPlayer.NickName;

        // Look for a GameObject with the same name as the local player's nickname
        GameObject localPlayerObject = GameObject.Find(localPlayerNickname);

        if (localPlayerObject != null)
        {
            // Destroy the local player's GameObject
            Destroy(localPlayerObject);
            Debug.Log($"Removed local player's prefab: {localPlayerNickname}");
        }
        else
        {
            Debug.LogWarning($"No GameObject found with the name: {localPlayerNickname}");
        }
    }


    /// <summary>
    /// Sync timer for late joiners when room properties are updated.
    /// </summary>
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(TimerStartTimeKey))
        {
            startTime = (double)propertiesThatChanged[TimerStartTimeKey];
            timerStarted = true;

            Debug.Log($"Timer updated for late joiners. StartTime: {startTime}");
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // Debug assigned jobs
        if (changedProps.ContainsKey("Job"))
        {
            Debug.Log($"Player {targetPlayer.NickName} assigned job: {changedProps["Job"]}");
        }
    }
}
