using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Start()
    {
        // Ensure the local player's nickname is set before instantiation
        if (string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName))
        {
            PhotonNetwork.LocalPlayer.NickName = "Player_" + Random.Range(1000, 9999).ToString();
        }

        // Generate a random spawn position
        Vector2 randomVector = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        // Instantiate the player object over the network
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomVector, Quaternion.identity);

        // Assign the player's nickname to the instantiated object
        player.name = PhotonNetwork.LocalPlayer.NickName;

        // Optionally, assign the player object to TagObject for easier access later
        PhotonNetwork.LocalPlayer.TagObject = player.transform;

        // Check if this player is the local player
        if (player.GetComponent<PhotonView>().IsMine)
        {
            // Get the main camera
            Camera mainCamera = Camera.main;

            // Attach the CameraFollow script and set the target to the local player
            if (mainCamera != null)
            {
                CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
                if (cameraFollow == null)
                {
                    cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
                }
                cameraFollow.target = player.transform;
                cameraFollow.offset = new Vector3(0, 0, -10); // Adjust as needed
            }
        }
    }
}
