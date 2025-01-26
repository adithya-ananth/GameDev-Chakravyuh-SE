using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PointsManager : MonoBehaviourPunCallbacks
{
    public TMP_Text pointsText;            // Text to display points
    private const string PointsKey = "Points"; // Key for points property

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PointsKey))
        {
            InitializePoints();
        }
        UpdatePointsDisplay(); // Display points on game start
    }

    /// <summary>
    /// Initialize the points property in the room.
    /// This is called only by the master client.
    /// </summary>
    private void InitializePoints()
    {
        var roomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { PointsKey, 0 } // Initialize points to 0
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        Debug.Log("Points initialized to 0.");
    }

    /// <summary>
    /// Increment points in room properties.
    /// Only the master client should call this.
    /// </summary>
    public static void IncrementPoints(int amount)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int currentPoints = (int)PhotonNetwork.CurrentRoom.CustomProperties[PointsKey];
        int newPoints = currentPoints + amount;

        var roomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { PointsKey, newPoints }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        Debug.Log($"Points updated to {newPoints}.");
    }

    /// <summary>
    /// Update the points display on the UI.
    /// </summary>
    private void UpdatePointsDisplay()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PointsKey))
        {
            int points = (int)PhotonNetwork.CurrentRoom.CustomProperties[PointsKey];
            pointsText.text = $"Points: {points}";
        }
    }

    /// <summary>
    /// Sync points display when room properties are updated.
    /// </summary>
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(PointsKey))
        {
            UpdatePointsDisplay();
        }
    }
}
