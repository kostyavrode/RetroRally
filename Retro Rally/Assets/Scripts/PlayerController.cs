using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    private PhotonView photonView;
    private void Awake()
    {
        //Instantiate(cars[Random.Range(0, cars.Length - 1)],gameObject.transform);
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (!photonView.IsMine)
        {
            //GetComponentInChildren<RCC_CarControllerV3>().isOther = true;
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<RCC_CarControllerV3>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }
}
