using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
