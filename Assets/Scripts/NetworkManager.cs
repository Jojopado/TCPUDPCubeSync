using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using MyCustom.NetworkServiceFw;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    byte eventCode = 1;
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
        roomOptions.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);

    }
    public override void OnJoinedRoom()
    {
        // 在玩家加入房間後綁定事件
        NetworkServiceFw.BindEvent<string, string, float, float>(eventCode, RE_TellOthersToSpawnMe);

        Debug.Log("One player has landed, but not spawned yet");
        Player localPlayer = PhotonNetwork.LocalPlayer;
        NetworkServiceFw.TriggerTCPToAll(eventCode, new object[] { localPlayer.UserId, localPlayer.NickName, 1.0f, 1.0f  });
        NetworkServiceFw.OnChangingGameScene(2);
    }

    /// <summary>
    /// string _id, string _nickName, float _power, float _speed
    /// </summary>
    /// <param name="_id"></param>
    void RE_TellOthersToSpawnMe(string _id, string _nickName, float _power, float _speed)
    {
        //past id, nickname to other players
        InGameManager.PlayerDataStructList.Add(new PlayerData.PlayerInfo { userID = _id, nickName = _nickName, power = _power, speed = _speed });
    }
}
