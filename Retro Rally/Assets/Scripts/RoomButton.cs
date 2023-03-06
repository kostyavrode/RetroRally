using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class RoomButton : MonoBehaviour
{
    [SerializeField] public TMP_Text roomName;
    public Photon.Realtime.RoomInfo info;
    public void SetUp(Photon.Realtime.RoomInfo roomInfo)
    {
        info = roomInfo;
        roomName.text = info.Name;
    }
    public void OnClick()
    {
        ServerConnection.instance.JoinRoom(info);
    }
}
