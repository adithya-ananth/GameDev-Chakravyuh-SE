using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class JobDisplay : MonoBehaviourPunCallbacks
{
    public TMP_Text jobText; // Reference to the TMP_Text UI component

    private void Start()
    {
        // Check and display the job when the scene loads
        UpdateJobDisplay();
    }

    /// <summary>
    /// Updates the TMP_Text to show the player's job.
    /// </summary>
    private void UpdateJobDisplay()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Job"))
        {
            string job = PhotonNetwork.LocalPlayer.CustomProperties["Job"].ToString();
            jobText.text = $"{job}";
        }
        else
        {
            jobText.text = "Your Job: Unassigned";
        }
    }

    /// <summary>
    /// Called whenever the player's CustomProperties are updated.
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.IsLocal && changedProps.ContainsKey("Job"))
        {
            // Update the job display for the local player
            UpdateJobDisplay();
        }
    }
}
