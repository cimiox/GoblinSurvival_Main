using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SyncHealth : Photon.MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        if (Player == null)
        {
            //if (!photonView.isMine)
            //{
            //    PhotonNetwork.Destroy(gameObject);
            //}
            //PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(GetComponent<Slider>().value);
        }
        else
        {
            GetComponent<Slider>().value = (float)stream.ReceiveNext();
        }
    }
}
