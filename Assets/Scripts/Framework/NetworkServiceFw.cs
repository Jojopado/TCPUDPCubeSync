using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System;

namespace MyCustom.NetworkServiceFw
{
    public class NetworkServiceFw : MonoBehaviour
    {
        [Tooltip("Bind event to method, first paran is custom event code, second is the method")]
        public static void BindEvent<T>(byte eventCodeIn, Action<T> _myFunc)
        {
            PhotonNetwork.NetworkingClient.EventReceived += (eventData) =>
            {
                if (eventData.Code == eventCodeIn)
                {
                    Debug.Log($"Received Event: Code = {eventCodeIn}");

                    try
                    {
                        // 檢查
                        if (eventData.CustomData is T eventDataPayload)
                        {
                            _myFunc.Invoke(eventDataPayload);
                        }
                        else
                        {
                            Debug.LogError($"Failed to cast CustomData to {typeof(T)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception during event processing: {ex.Message}");
                    }
                }
            };
        }

        public static void BindEvent<T1, T2, T3>(byte eventCodeIn, Action<T1, T2, T3> _myFunc)
        {
            //  記得寫ㄏㄚˊ
        }
        [Tooltip("Using RaiseEvent to send data to all players, Important! including the sender can recieved the event.")]
        //params 裡面的參數就是要丟的資料，用object[]包起來有需要再轉type
        public static void TriggerTCPToAll(params object[] _raiseEventData)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ConnectionProtocol.Tcp;
            RaiseEventOptions _raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent((byte)_raiseEventData[0], _raiseEventData[1], _raiseEventOptions, SendOptions.SendReliable);
        }
        public static void TriggerTCPToOthers(params object[] _raiseEventData)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ConnectionProtocol.Tcp;
            RaiseEventOptions _raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((byte)_raiseEventData[0], _raiseEventData[1], _raiseEventOptions, SendOptions.SendReliable);
        }
        public static void TriggerUdpToAll(params object[] _raiseEventData)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ConnectionProtocol.Udp;
            RaiseEventOptions _raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent((byte)_raiseEventData[0], _raiseEventData[1], _raiseEventOptions, SendOptions.SendUnreliable);
        }
        public static void TriggerUdpToOthers(params object[] _raiseEventData)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ConnectionProtocol.Udp;
            RaiseEventOptions _raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((byte)_raiseEventData[0], _raiseEventData[1], _raiseEventOptions, SendOptions.SendUnreliable);
        }
        public static void OnChangingGameScene(int roomIndex)
        {
            PhotonNetwork.LoadLevel(roomIndex);
        }
    }
}