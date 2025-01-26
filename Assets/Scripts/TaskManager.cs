using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    // Dictionary to map task names to required jobs
    private Dictionary<string, string> taskRequirements = new Dictionary<string, string>
    {
        { "TaskSceneAgainstSDG1", "Saboteur" },
        { "game", "Collaborator" },
        { "Ocean", "Saboteur" },
        { "Deforestation", "Saboteur" },
        { "BoatTrash", "Collaborator" },
        { "ForSceneSDG1", "Collaborator" },
        { "ForSceneSDG3", "Collaborator" },
        { "IntroQuiz", "Collaborator" },
        { "TreePlanting", "Collaborator" },
    };

    /// <summary>
    /// Called when a task button is clicked.
    /// </summary>
    /// <param name="taskname">The name of the task scene to load.</param>
    public void OnTaskButtonClick(string taskname)
    {
        // Check if the player's job matches the task's requirement
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Job"))
        {
            string playerJob = PhotonNetwork.LocalPlayer.CustomProperties["Job"].ToString();

            if (taskRequirements.ContainsKey(taskname))
            {
                string requiredJob = taskRequirements[taskname];

                if (playerJob == requiredJob)
                {
                    // Remove the local player's prefab and load the task scene
                    MainTimer.RemoveLocalPlayerPrefab();
                    PhotonNetwork.LoadLevel(taskname);
                }
                else
                {
                    Debug.LogError($"Task '{taskname}' requires '{requiredJob}', but your job is '{playerJob}'.");
                }
            }
            else
            {
                Debug.LogError($"Task '{taskname}' does not exist in the taskRequirements dictionary.");
            }
        }
        else
        {
            Debug.LogError("Player job not assigned yet!");
        }
    }
}
