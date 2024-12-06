using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MyCustom.NetworkServiceFw;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Threading.Tasks;
public class InGameManager : MonoBehaviour
{
    GameObject playerPrefab;

    //this is the local player 
    GameObject localPlayer;

    GameObject playerOffset;

    byte eventCode_UpdatePlayerList = 2;


    #region  Hide

    /// <summary>
    /// 封裝的玩家資料
    /// </summary>
    /// This data has to be broardcasted continuously
    public static List<PlayerData.PlayerInfo> PlayerDataStructList = new List<PlayerData.PlayerInfo>();


    public List<PlayerData.PlayerInfo> _PlayerDataStructList;
    public List<GameObject> playerCubeList;


    int currentScenePlayerCount = 0;
    #endregion
    void OnEnable()
    {
        NetworkServiceFw.BindEvent(eventCode_UpdatePlayerList, SyncAcrossAllPlayerList);

    }
    void OnDisable()
    {

    }
    void Start()
    {
        //如果不延遲抓不到資料
        playerPrefab = (GameObject)Resources.Load("CubePlayer");
        SpawnAndSetDataMySelfAsync();
    }

    // Update is called once per frame
    void Update()
    {
        playerOffset = localPlayer;

        foreach (var item in PlayerDataStructList)
        {
            Debug.Log(item.nickName + item.position);
        }
        //先進來的玩家可以抓到第二個玩家進入
        if (PhotonNetwork.IsMasterClient)
        {
            NetworkServiceFw.TriggerTCPToAll(eventCode_UpdatePlayerList, null);
            //Debug.Log("Master client is sending data to all players");
        }
        
        //add player until match the player count
        if (currentScenePlayerCount < _PlayerDataStructList.Count)
        {
            //Debug.Log($"currentScenePlayerCount {currentScenePlayerCount}");
            //Debug.Log($"_PlayerDataStructList.Count {_PlayerDataStructList.Count}");
            SpawnOtherPlayer();
        }
        //UpdateOtherPlayerPosition();
    }
    async void SpawnAndSetDataMySelfAsync()
    {
        //家自己
        currentScenePlayerCount++;
        await Task.Delay(3000);

        localPlayer = Instantiate(playerPrefab, new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4)), Quaternion.identity);
        playerCubeList.Add(localPlayer);
        //player.AddComponent<PlayerData>();
        localPlayer.GetComponent<PlayerData>()._playerInfo.userID = PlayerDataStructList[PlayerDataStructList.Count - 1].userID;
        localPlayer.GetComponent<PlayerData>()._playerInfo.nickName = PlayerDataStructList[PlayerDataStructList.Count - 1].nickName;
        localPlayer.GetComponent<PlayerData>()._playerInfo.power = PlayerDataStructList[PlayerDataStructList.Count - 1].power;
        localPlayer.GetComponent<PlayerData>()._playerInfo.speed = PlayerDataStructList[PlayerDataStructList.Count - 1].speed;
    }
    void SpawnOtherPlayer()
    {
        GameObject thePlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        thePlayer.GetComponent<PlayerData>()._playerInfo = _PlayerDataStructList[currentScenePlayerCount];
        //remove不然會控制到別的玩家
        thePlayer.GetComponent<PlayerControllerCube>().enabled = false;
        playerCubeList.Add(thePlayer);
        currentScenePlayerCount++;
    }
    void UpdateOtherPlayerPosition()
    {
        for (int i = 0; i < _PlayerDataStructList.Count; i++)
        {
            var playerInfo = _PlayerDataStructList[i];
            var playerObject = playerCubeList
                .Find(cube => cube.GetComponent<PlayerData>()._playerInfo.userID == playerInfo.userID);

            if (playerObject != null)
            {
                playerObject.GetComponent<PlayerData>()._playerInfo.position = playerInfo.position;
                _PlayerDataStructList[i] = playerInfo; // Update the list if needed
            }
        }
    }



    /// <summary>
    /// 避免後進入玩家無法抓到先進入玩家的資料，所以由所有玩家同時同步資料
    /// </summary>
    void SyncAcrossAllPlayerList()
    {
        //嗚嗚嗚好感動，卡了很久，終於解決了。後進來的玩家抓不到先進來的玩家資料，原因是出在要用cache(options)
        _PlayerDataStructList = PlayerDataStructList;
        //Debug.Log($"_PlayerDataStructList {_PlayerDataStructList[0].position}");
        Debug.Log("Updating");
    }

}
