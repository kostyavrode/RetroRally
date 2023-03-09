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
        if (!photonView.IsMine)
        {
            //GetComponentInChildren<RCC_CarControllerV3>().isOther = true;
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<RCC_CarControllerV3>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            GetComponentInChildren<RCC_MobileButtons>().gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        RaceManager.onRaceState += SetMobileInput;
    }
    private void OnDisable()
    {
        RaceManager.onRaceState -= SetMobileInput;
    }
    private void Start()
    {

    }
    private void SetMobileInput(bool boolka)
    {
        if (photonView.IsMine && Application.isMobilePlatform)
        {
            RCC.SetController(1);
        }
    }
}
