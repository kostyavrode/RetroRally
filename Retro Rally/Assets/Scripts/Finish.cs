using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
public class Finish : MonoBehaviour
{
    public static Action<string, int> onFinishReached;
    private List<GameObject> finishedPlayers = new List<GameObject>();
    private int playersCount;
    private void Start()
    {
        playersCount = PhotonNetwork.PlayerList.Length;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PhotonView photonView)&&!IsPlayerAlreadyFinished(other.gameObject))
        {
            finishedPlayers.Add(other.gameObject);
            onFinishReached?.Invoke(photonView.Owner.NickName,finishedPlayers.Count);
        }
    }
    private bool IsPlayerAlreadyFinished(GameObject obj)
    {
        for (int i=0;i<finishedPlayers.Count;i++)
        {
            if (obj==finishedPlayers[i])
            {
                return true;
            }
            else
            {
                Debug.Log("Finish");
                return false;
            }
        }
        return false;
    }
}
