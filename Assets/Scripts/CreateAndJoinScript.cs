using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro; // Import TextMesh Pro namespace

public class CreateAndJoinScript : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput; // Use TMP_InputField
    public TMP_InputField joinInput;   // Use TMP_InputField

    public void CreateRoom() {
        RoomOptions roomOptions = new RoomOptions();
        PhotonNetwork.CreateRoom(createInput.text, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom() {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom() {
        //PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Joined Room");
         PhotonNetwork.LoadLevel("Map");
    }
}
