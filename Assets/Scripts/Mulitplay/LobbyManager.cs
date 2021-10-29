using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

# if PLATFORM_ANDROID
using UnityEngine.Android;
# endif



public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";


    public Text connectionInfoText;
    public Text Nickname_Input;
    public Button joinButton;

    // Start is called before the first frame update
    void Start()
    {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
                
            }
#endif

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        
        connectionInfoText.text = "master sever joinging...";
    }

    public override void OnConnectedToMaster()
    {

        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected with Master Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "Offline : Disconnected wiht Master Server\nReconnect with Server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinButton.interactable = false;
        PhotonNetwork.NickName = Nickname_Input.text;
        
        Debug.Log("nick : " + PhotonNetwork.NickName);

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connect to Room";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "Offline : Disconnected wiht Master Server\nReconnect with Server...";
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "No empty room, new room created";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Enter Room Success";
        PhotonNetwork.LoadLevel("SurroundFruits");
    }
}
