using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using UniRx;
public class RaceManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static Action<bool> onRaceState;
    private PhotonView photonView;
    private const int DELAY = 3;
    private TMP_Text timeBar;
    private IDisposable delayTimer;
    private IDisposable timer;
    private int time;
    private int finishTime;
    private int timeDispalyed;
    private void OnEnable()
    {
        RoomManager.onGameStarted += ShowDelayTime;
        //ShowDelayTime();
        onRaceState += StartTimer;
        Finish.onFinishReached += ShowTime;
    }
    private void OnDisable()
    {
        RoomManager.onGameStarted -= ShowDelayTime;
        Finish.onFinishReached -= ShowTime;
        onRaceState -= StartTimer;
    }
    private void OnDisconnectedFromServer()
    {
        delayTimer?.Dispose();
    }
    private void ShowDelayTime()
    {
        photonView = GetComponent<PhotonView>();
        timeBar = GameObject.FindGameObjectWithTag("Timer").GetComponent<TMP_Text>();
        if (!PhotonNetwork.IsMasterClient)
        {
            delayTimer = Observable.EveryUpdate().Subscribe(x =>
            {
                timeBar.text = time.ToString();
            });
            return;
        }
        int i=0;
        delayTimer = Observable.Interval(System.TimeSpan.FromSeconds(1)).Subscribe(x =>
          {
              time = DELAY - i;
              timeBar.text = time.ToString();
              i++;
              if (i==4)
              {
                  photonView.RPC("StartRace", RpcTarget.All);
              }
          });
    }
    [PunRPC]
    private void StartRace()
    {
        timeBar.enabled = false;
        delayTimer?.Dispose();
        onRaceState?.Invoke(true);
    }
    private void StartTimer(bool state)
    {
        if (true && PhotonNetwork.IsMasterClient)
        timer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>
          {
              time += 1;
          });
    }
    [PunRPC]
    private void ShowTime(string name, int playersFinished)
    {
        if (timeDispalyed < 3)
        {
            timeDispalyed++;
            if (playersFinished == 2)
            {
                onRaceState?.Invoke(false);
                timer?.Dispose();
            }
            else
                timeBar.text = "";
            timeBar.enabled = true;
            if (PhotonNetwork.IsMasterClient)
            {
                finishTime = time;
                timeBar.text += name + " time: " + finishTime.ToString() + "s\n";

            }
            else
            {
                timeBar.text += name + " time: " + finishTime.ToString() + "s\n";
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(time);
            stream.SendNext(finishTime);
        }
        else if (stream.IsReading)
        {
            finishTime = (int)stream.ReceiveNext();
            time = (int)stream.ReceiveNext();
            //timeBar.text = time.ToString();
        }
    }
}
