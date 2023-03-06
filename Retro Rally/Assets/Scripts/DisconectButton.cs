using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class DisconectButton : MonoBehaviourPunCallbacks
{
    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();  



        //GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().enabled = false;
    }
    public override void OnLeftRoom()
    {
        Destroy(GameObject.FindGameObjectWithTag("RoomManager"));
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in gameObjects)
        {
            Destroy(obj);
        }
        SceneManager.LoadScene(0);
    }
}
