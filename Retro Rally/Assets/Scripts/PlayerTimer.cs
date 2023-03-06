using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeBar;
    private float time;
    private bool isRaceStarted;
    private void Awake()
    {
        if (timeBar==null)
        {
            timeBar=GetComponent<TMP_Text>();
        }
        
    }
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        time++;
    }
    private void StartTimer()
    {
        isRaceStarted = true;
    }

}
