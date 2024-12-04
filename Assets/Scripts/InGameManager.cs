using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MyCustom.NetworkServiceFw;
using ExitGames.Client.Photon;
using Photon.Realtime;
public class InGameManager : MonoBehaviour
{
    GameObject playerPrefab;
    byte eventCode = 1;
    void Start()
    {
        //spawn the player with nick name on it
        //PhotonNetwork.Instantiate requires PhotonView component QAQ
        //PhotonNetwork.Instantiate("CubePlayer",new Vector3(0,0,0),Quaternion.identity);

        playerPrefab = Resources.Load("CubePlayer") as GameObject;
        object[] content = new object[] { playerPrefab };
        PhotonNetwork.RaiseEvent(eventCode, content, RaiseEventOptions.Default, SendOptions.SendReliable);
        


        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnSpawningPlayer(EventData photonEvent)
    {
        if (photonEvent.Code == eventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Instantiate((GameObject)data[0]);
        }
    }
}
