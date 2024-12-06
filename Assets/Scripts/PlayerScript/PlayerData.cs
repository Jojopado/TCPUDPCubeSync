using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCustom.NetworkServiceFw;
public class PlayerData : MonoBehaviour
{
    [SerializeField]
    public PlayerInfo _playerInfo;

    [System.Serializable]
    public struct PlayerInfo
    {
        public string userID;
        public string nickName;
        public float power;
        public float speed;
        public Vector3 position;
        public Vector3 rotation;
    }
    void Update()
    {
        _playerInfo.position = transform.position;
        _playerInfo.rotation = transform.rotation.eulerAngles;   
    }
}
