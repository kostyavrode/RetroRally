using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkPlayer : MonoBehaviour, IPunObservable
{
    protected RCC_CarControllerV3 Player;
    protected Vector3 RemotePlayerPosition;
    protected float RemoteLookX;
    protected float RemoteLookZ;
    protected float LookXVel;
    protected float LookZVel;
    private PhotonView photonView;

    private void Awake()
    {
        Player = GetComponentInChildren<RCC_CarControllerV3>();

        //destroy the controller if the player is not controlled by me
        if (!GetComponent<PhotonView>().IsMine)
            photonView = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (photonView.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        //High distance => sync is to much off => send to position
        if (LagDistance.magnitude > 5f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector3.zero;
        }

        //ignore the y distance
        LagDistance.y = 0;

        if (LagDistance.magnitude < 0.11f)
        {
            //Player is nearly at the point
            Player.gasInput = 0;
            Player.steerInput = 0;
        }
        else
        {
            //Player has to go to the point
            Player.gasInput = LagDistance.normalized.x;
            Player.steerInput = LagDistance.normalized.z;
        }

        //jump if the remote player is higher than the player on the current client

        //Look Smooth

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
