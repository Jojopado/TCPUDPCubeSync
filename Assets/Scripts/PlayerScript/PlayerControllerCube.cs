using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCustom.NetworkServiceFw;

public class PlayerControllerCube : MonoBehaviour
{
    private Vector3 goVector;

    //先改public看有沒有抓到
    public InGameManager _inGameManager;
    //先改public看有沒有抓到
    public PlayerData _playerData;

    byte eventCode_UploadSelfTransform = 3;



    void OnEnable()
    {
        // Sync Vector3就好
        

    }
    void Start()
    {
        //cubeDemo = FindObjectOfType<CubeDemoShowCaseManager>();
        NetworkServiceFw.BindEvent(eventCode_UploadSelfTransform, SyncLocalPlayerTransform);
        _inGameManager = FindObjectOfType<InGameManager>();
        _playerData = this.gameObject.GetComponent<PlayerData>();
    }

    void Update()
    {
        goVector = transform.position;
        if (_playerData._playerInfo.userID == Photon.Pun.PhotonNetwork.LocalPlayer.UserId)
        {
            Debug.Log("It's me");
            if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.position += Vector3.left * 0.01f;
                NetworkServiceFw.TriggerUdpToAll(eventCode_UploadSelfTransform, null);
            }
            if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.position += Vector3.right * 0.01f;
                NetworkServiceFw.TriggerUdpToAll(eventCode_UploadSelfTransform, null);
            }
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.transform.position += Vector3.forward * 0.01f;
                NetworkServiceFw.TriggerUdpToAll(eventCode_UploadSelfTransform, null);
            }
            if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.position += Vector3.back * 0.01f;
                NetworkServiceFw.TriggerUdpToAll(eventCode_UploadSelfTransform, null);
            }
        }
        else
        {
            Debug.Log("It's not me");
        }

    }
    void SyncLocalPlayerTransform()
    {
        var playerIndex = InGameManager.PlayerDataStructList.FindIndex(player => player.userID == _playerData._playerInfo.userID);

        if (playerIndex == -1)
        {
            
            Debug.LogError($"Player with userID {_playerData._playerInfo.userID} not found in PlayerDataStructList.");
            return;
        }
        //從List撈出自己
        var updatedPlayerData =InGameManager.PlayerDataStructList[playerIndex];
        //自己的位置更新
        updatedPlayerData.position = gameObject.transform.position;
        //把自己放回去
        InGameManager.PlayerDataStructList[playerIndex] = updatedPlayerData;
        
        //InGameManager.PlayerDataStructList = new List<PlayerData.PlayerInfo>(_inGameManager._PlayerDataStructList);
    }


}
