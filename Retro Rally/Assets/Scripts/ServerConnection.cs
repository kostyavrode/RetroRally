using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    public static ServerConnection instance;
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Transform roomsListTransform;
    [SerializeField] private GameObject roomButtonPrefab;
    [SerializeField] private Transform playerListTransform;
    [SerializeField] private GameObject playerNamePrefab;
    [SerializeField] private GameObject startButtonPrefab;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        OnlineMenuManager.instance.OpenMenu("loading");
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to lobby");
        //PhotonNetwork.NickName = "Player" + Random.Range(0, 10000).ToString(format: "00000");
        if (PhotonNetwork.NickName=="")
        OnlineMenuManager.instance.OpenMenu("setNickname");
        else
        {
            OnlineMenuManager.instance.OpenMenu("title");
        }
    }
    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInputField.text) && roomNameInputField.text.Length<10)
        {
            PhotonNetwork.CreateRoom(roomNameInputField.text);
            OnlineMenuManager.instance.OpenMenu("loading");
        }
        else
        {
            OnlineMenuManager.instance.OpenMenu("error");
        }
    }
    public override void OnJoinedRoom()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        for (int i=0;i<playerListTransform.childCount;i++)
        {
            Destroy(playerListTransform.GetChild(i).gameObject);
        }
        for (int i=0;i<players.Length;i++)
        {
            Instantiate(playerNamePrefab, playerListTransform).GetComponent<PlayerName>().SetuUp(players[i]);
        }
        startButtonPrefab.SetActive(PhotonNetwork.IsMasterClient);
        OnlineMenuManager.instance.OpenMenu("roomMenu");
    }
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        startButtonPrefab.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        OnlineMenuManager.instance.OpenMenu("error");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        OnlineMenuManager.instance.OpenMenu("loading");
    }
    public override void OnLeftRoom()
    {
        OnlineMenuManager.instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
    {
        for (int i=0; i<roomsListTransform.childCount;i++)
        {
            Destroy(roomsListTransform.GetChild(i).gameObject);
        }
        for (int i=0; i<roomList.Count;i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomButtonPrefab, roomsListTransform).GetComponent<RoomButton>().SetUp(roomList[i]);
        }
    }
    public void JoinRoom(Photon.Realtime.RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        OnlineMenuManager.instance.OpenMenu("loading");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Instantiate(playerNamePrefab, playerListTransform).GetComponent<PlayerName>().SetuUp(newPlayer);
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void SetNickname()
    {
        string name = nicknameInputField.text;
        if (CheckNickName(name))
        {
            PhotonNetwork.NickName = name;
            OnlineMenuManager.instance.OpenMenu("title");
        }
        else
        {
            OnlineMenuManager.instance.OpenMenu("error");
        }
        
    }
    private bool CheckNickName(string name)
    {
        bool tempBool = true;
        for(int i=0; i<PhotonNetwork.PlayerList.Length;i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName==name)
            {
                tempBool=false;
            }
            else
            {
                tempBool = true;
            }
        }
        if (name.Length > 10)
        {
            return false;
        }
        return tempBool;
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
