using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class TrackSpawner : MonoBehaviour
{
    //[SerializeField] private TrackPart[] trackParts;
    //[SerializeField] private int countParts = 15;
    //private List<TrackPart> spawnedParts = new List<TrackPart>();
    //private Vector3 currentPosition= Vector3.zero;
    //private void Start()
    //{
    //    if (trackParts.Length>0)
    //    {
    //        for (int i=0; i<countParts;i++)
    //        {
    //            spawnedParts.Add(GetTrackPart());
    //            SpawnTrackPart(spawnedParts[i], GetSpawnPosition(spawnedParts[i]));
    //        }
    //    }
    //}
    //private TrackPart GetTrackPart()
    //{
    //    return trackParts[Random.Range(0, trackParts.Length)];
    //}
    //private Vector3 GetSpawnPosition(TrackPart lastPart)
    //{
    //    if (lastPart.name==TrackPart.TrackNames.forward)
    //    return currentPosition=new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 80);
    //    if (lastPart.name == TrackPart.TrackNames.left)
    //        return currentPosition = new Vector3(currentPosition.x-80, currentPosition.y, currentPosition.z);
    //    if (lastPart.name == TrackPart.TrackNames.right)
    //        return currentPosition = new Vector3(currentPosition.x + 80, currentPosition.y, currentPosition.z);
    //    else return Vector3.zero;
    //}
    //private void SpawnTrackPart(TrackPart part, Vector3 spawnPosition)
    //{
    //    TrackPart newPart=Instantiate(part, this.transform);
    //    newPart.transform.position = spawnPosition;
    //}
    [SerializeField] private GameObject tempObjectPrefab;
    [SerializeField] private TrackPart[] trackParts;
    [SerializeField] private int countParts = 15;
    private List<TrackPart> spawnedParts = new List<TrackPart>();
    private List<GameObject> tempParts = new List<GameObject>();
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 tempPosition = Vector3.zero;
    private Vector3 checkVector = new Vector3(10f, 10f, 10f);
    private Vector3 lastTempPosition;
    private IDisposable tempCreator;
    private void Start()
    {
        int i = 0;
        tempCreator = Observable.Interval(System.TimeSpan.FromMilliseconds(10)).Subscribe(x =>
        {
            SpawnTempObject(i);
            i++;
            if (i==countParts)
            {
                SpawnRealLevel();
                tempCreator?.Dispose();
            }
        });

    }
    private void SpawnTempObject(int i)
    {
        lastTempPosition = tempPosition;
        int temp = UnityEngine.Random.Range(0, 4);
        switch (temp)
        {
            case 0:
                {
                    tempPosition = new Vector3(tempPosition.x, tempPosition.y, tempPosition.z+80f);
                }
                break;
            case 1:
                {
                    tempPosition = new Vector3(tempPosition.x+80f, tempPosition.y, tempPosition.z);
                }
                break;
            case 2:
                {
                    tempPosition = new Vector3(tempPosition.x - 80f, tempPosition.y, tempPosition.z);
                }
                break;
            case 3:
                {
                    tempPosition = new Vector3(tempPosition.x, tempPosition.y, tempPosition.z-80f);
                }
                break;
        }
        if (i == 0)
        {
            tempPosition = new Vector3(0, tempPosition.y, tempPosition.z + 80f);
        }
        if (CheckSpawnPoint(tempPosition))
        {
            GameObject newObj = Instantiate(tempObjectPrefab);
            newObj.transform.position = tempPosition;
            lastTempPosition = tempPosition;
            tempParts.Add(newObj);
        }
        else
        {
            SpawnTempObject(1);
        }
    }
    private bool CheckSpawnPoint(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, checkVector);
        Debug.Log(colliders.Length+ "    "+position);
        if (colliders.Length>0)
        {
            tempPosition = lastTempPosition;
            return false;
        }
        else
        {
            //tempPosition = lastTempPosition;
            return true;
        }
    }
    private void SpawnRealLevel()
    {
        for (int i=0;i<tempParts.Count; i++)
        {
            Debug.Log("SpawnRealLevel");
            if (i == 0)
            {
                spawnedParts.Add((Instantiate(trackParts[2])));
                spawnedParts[i].transform.position = tempParts[i].transform.position;
            }
            else
            {
                try
                    {

                    if (tempParts[i].transform.position.x == tempParts[i - 1].transform.position.x && tempParts[i + 1].transform.position.x > tempParts[i].transform.position.x)
                    {
                        spawnedParts.Add((Instantiate(trackParts[1])));
                        spawnedParts[i].transform.position = tempParts[i].transform.position;
                    }
                    if ((tempParts[i - 1].transform.position.x > tempParts[i].transform.position.x && tempParts[i - 1].transform.position.z == tempParts[i].transform.position.z) && (tempParts[i].transform.position.x == tempParts[i + 1].transform.position.x && tempParts[i].transform.position.z > tempParts[i + 1].transform.position.z))
                    {
                        spawnedParts.Add((Instantiate(trackParts[0])));
                        spawnedParts[i].transform.position = tempParts[i].transform.position;
                    }
                    if ((tempParts[i - 1].transform.position.x < tempParts[i].transform.position.x && tempParts[i - 1].transform.position.z == tempParts[i].transform.position.z) && (tempParts[i].transform.position.x == tempParts[i + 1].transform.position.x && tempParts[i].transform.position.z < tempParts[i + 1].transform.position.z))
                    {
                        spawnedParts.Add((Instantiate(trackParts[0])));
                        spawnedParts[i].transform.position = tempParts[i].transform.position;
                    }
                    if ((tempParts[i - 1].transform.position.x == tempParts[i].transform.position.x && tempParts[i - 1].transform.position.z > tempParts[i].transform.position.z) && (tempParts[i].transform.position.x < tempParts[i + 1].transform.position.x && tempParts[i].transform.position.z == tempParts[i + 1].transform.position.z))
                    {
                        spawnedParts.Add((Instantiate(trackParts[1])));
                        spawnedParts[i].transform.position = tempParts[i].transform.position;
                    }
                    else
                    {
                        spawnedParts.Add((Instantiate(trackParts[2])));
                        spawnedParts[i].transform.position = tempParts[i].transform.position;
                    }    
                }
                catch
                {
                    spawnedParts.Add(Instantiate(trackParts[2]));
                    spawnedParts[i].transform.position = tempParts[i].transform.position;
                }
            }
        }
    }
}
