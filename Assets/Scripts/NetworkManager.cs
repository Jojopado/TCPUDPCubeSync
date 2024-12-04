using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using MyCustom.NetworkServiceFw;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        TryToConnect();
    }

    void TryToConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try to connect to server...");
    }
    public override void OnConnectedToMaster()
    {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);

    }
    public override void OnJoinedRoom()
    {
        NetworkServiceFw.OnChangingGameScene(2);
        Debug.Log("One player has landed!");
    }
}
