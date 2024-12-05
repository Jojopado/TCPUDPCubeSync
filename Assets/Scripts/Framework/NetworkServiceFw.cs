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
        public static void BindEvent(byte eventCodeIn, Action _myFunc)
        {
            PhotonNetwork.NetworkingClient.EventReceived += (eventData) =>
            {
                if (eventData.Code == eventCodeIn)
                {
                    Debug.Log($"Received Event: Code = {eventCodeIn}");

                    try
                    {
                        // Check if CustomData is null or not needed
                        if (eventData.CustomData == null)
                        {
                            _myFunc.Invoke();
                        }
                        else
                        {
                            Debug.LogError("CustomData should be null for this event type.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception during event processing: {ex.Message}");
                    }
                }
            };
        }
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

        public static void BindEvent<T1, T2, T3, T4>(byte eventCodeIn, Action<T1, T2, T3, T4> _myFunc)
        {
            PhotonNetwork.NetworkingClient.EventReceived += (eventData) =>
            {
                if (eventData.Code == eventCodeIn)
                {
                    //Debug.Log($"Received Event: Code = {eventCodeIn}");

                    try
                    {
                        // Check if CustomData is an array of objects
                        /*
                         if (eventData.CustomData is T1 eventDataPayload && eventData.CustomData is T2 eventDataPayload2 && eventData.CustomData is T3 eventDataPayload3 && eventData.CustomData is T4 eventDataPayload4)
                        {1+
                            _myFunc.Invoke(eventDataPayload, eventDataPayload2, eventDataPayload3, eventDataPayload4);
                        }
                        */
                        if (eventData.CustomData is object[] dataArray &&
                            dataArray.Length == 4 &&
                            dataArray[0] is T1 payload1 &&
                            dataArray[1] is T2 payload2 &&
                            dataArray[2] is T3 payload3 &&
                            dataArray[3] is T4 payload4)
                        {
                            _myFunc.Invoke(payload1, payload2, payload3, payload4);
                        }
                        else
                        {
                            Debug.LogError($"CustomData is not a valid array or doesn't match expected types.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception during event processing: {ex.Message}");
                    }
                }
            };
        }

        [Tooltip("Using RaiseEvent to send data to all players, Important! including the sender can recieved the event.")]
        //params 裡面的參數就是要丟的資料，用object[]包起來有需要再轉type
        public static void TriggerTCPToAll(params object[] _raiseEventData)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ConnectionProtocol.Tcp;
            RaiseEventOptions _raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            //CachingOption先家在這邊
            _raiseEventOptions.CachingOption = EventCaching.AddToRoomCacheGlobal;
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