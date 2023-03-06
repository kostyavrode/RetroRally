using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    private List<Transform> spawnPositions= new List<Transform>();
    private PhotonView photonView;
    int i;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        GetSpawnPositions();
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            CreateController();
        }
    }
    private void CreateController()
    {
        if (i<1)
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"),SpawnPosition(),Quaternion.identity);
        i++;
    }
    private void GetSpawnPositions()
    {
        GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("SpawnPos");
        foreach (GameObject obj in tempObjects)
        {
            spawnPositions.Add(obj.transform);
        }
    }
    private Vector3 SpawnPosition()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return spawnPositions[0].position;
        }
        else
            return spawnPositions[UnityEngine.Random.Range(1, spawnPositions.Count - 1)].position;
        //int lastIndex = spawnPositions.Count - 1;
        //Vector3 tempPos = spawnPositions[UnityEngine.Random.Range(0,spawnPositions.Count-1)].transform.position;
        //spawnPositions.RemoveAt(lastIndex);
        //Debug.Log(spawnPositions.Count);
        //return tempPos;
    }
}
