using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public List<string> acceptedTags; // List of tags that this trash bin accepts
    public int scoreIncrement = 10;

    public void HandleTrashDrop(GameObject trash)
    {
        if (acceptedTags.Contains(trash.tag)) // Check if the trash tag is in the accepted list
        {
            Destroy(trash);
            ScoreManager.instance.AddScore(scoreIncrement);
        }
        else
        {
            Debug.Log("Incorrect trash dropped!");
            Destroy(trash);
            ScoreManager.instance.AddScore(-5); // Optional penalty
        }
    }
}
