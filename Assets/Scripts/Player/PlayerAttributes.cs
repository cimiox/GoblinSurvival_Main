using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerAttributes : Photon.MonoBehaviour
{
    public GameObject HealthObject;

    public float Damage = 15;

    public float HealthPlayer = 100;

    void Start()
    {
        HealthObject = PhotonNetwork.Instantiate("HealthSlider", Vector3.one, Quaternion.identity, 0) as GameObject;

        HealthObject.GetComponent<SyncHealth>().Player = gameObject;

        HealthObject.transform.parent = GameObject.Find("Canvas").transform;


        HealthObject.GetComponent<Slider>().maxValue = HealthPlayer;
        HealthObject.GetComponent<Slider>().value = HealthPlayer;
    }

    void Update()
    {
        HealthObject.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(10f, 30f, 0f);
        HealthObject.name = string.Format(PhotonNetwork.playerName + " HealthSlider");

        if (HealthPlayer <= 0)
        {
            if (!photonView.isMine)
            {
                PhotonNetwork.Destroy(HealthObject);
            }

            PhotonNetwork.Destroy(photonView);

            if (photonView.isMine)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        GetComponent<PhotonView>().RPC("DamageTake", PhotonTargets.AllBuffered, damage);
    }

    [PunRPC]
    public void DamageTake(float damage)
    {
        Damage = damage;
        HealthPlayer -= Damage;

        HealthObject.GetComponent<Slider>().value = HealthPlayer;
    }
}


