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
    GameObject player;

    byte eventCode = 2;

    #region  Hide

    /// <summary>
    /// 封裝的玩家資料
    /// </summary>
    /// This data has to be broardcasted continuously
    public static List<PlayerData.PlayerInfo> PlayerDataStructList = new List<PlayerData.PlayerInfo>();


    public List<PlayerData.PlayerInfo> _PlayerDataStructList;
    #endregion
    void OnEnable()
    {
        NetworkServiceFw.BindEvent(eventCode, SyncAcrossAllPlayerList);
    }
    void OnDisable()
    {

    }
    void Start()
    {
        //如果不延遲抓不到資料
        SpawnAndSetDataMySelfAsync();
    }

    // Update is called once per frame
    void Update()
    {

        //先進來的玩家可以抓到第二個玩家進入
        //第二位玩家抓不到第一位玩家進入
        if (PhotonNetwork.IsMasterClient)
        {
            
            NetworkServiceFw.TriggerTCPToAll(eventCode, null);
            Debug.Log("Master client is sending data to all players");
        }

    }
    async void SpawnAndSetDataMySelfAsync()
    {
        await Task.Delay(3000);  // Wait for 3 seconds 
        playerPrefab = (GameObject)Resources.Load("CubePlayer");
        player = Instantiate(playerPrefab, new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4)), Quaternion.identity);
        //player.AddComponent<PlayerData>();
        player.GetComponent<PlayerData>()._playerInfo.userID = PlayerDataStructList[PlayerDataStructList.Count - 1].userID;
        player.GetComponent<PlayerData>()._playerInfo.nickName = PlayerDataStructList[PlayerDataStructList.Count - 1].nickName;
        player.GetComponent<PlayerData>()._playerInfo.power = PlayerDataStructList[PlayerDataStructList.Count - 1].power;
        player.GetComponent<PlayerData>()._playerInfo.speed = PlayerDataStructList[PlayerDataStructList.Count - 1].speed;
    }
    void SpawnOtherPlayer(string _id)
    {
        Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    /// <summary>
    /// 避免後進入玩家無法抓到先進入玩家的資料，所以由所有玩家同時同步資料
    /// </summary>
    void SyncAcrossAllPlayerList()
    {
        //嗚嗚嗚好感動，卡了很久，終於後進來的玩家也可以抓到先進來的玩家資料了，原因出在要用cache
        _PlayerDataStructList = new List<PlayerData.PlayerInfo>(PlayerDataStructList);
        Debug.Log("Scene player count " + _PlayerDataStructList.Count);
    }
}
