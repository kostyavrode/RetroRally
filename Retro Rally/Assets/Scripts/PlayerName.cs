using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerName : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text nameText;
    private Photon.Realtime.Player player;
    private void Awake()
    {
        nameText = GetComponent<TMP_Text>();
    }
    public void SetuUp(Photon.Realtime.Player person)
    {
        player = person;
        nameText.text = person.NickName;
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (player==otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
