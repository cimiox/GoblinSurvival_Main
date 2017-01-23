using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PlayerAttributes : Photon.MonoBehaviour
{
    public static PlayerAttributes Instance;

    public GameObject HealthObject;

    public float Health { get; set; }
    public float Damage { get; set; }

    private void Awake()
    {
        Health = 100;
        Damage = 100;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        HealthObject = PhotonNetwork.Instantiate("HealthSlider", Vector3.one, Quaternion.identity, 0) as GameObject;

        HealthObject.GetComponent<SyncHealth>().Player = gameObject;

        HealthObject.transform.parent = GameObject.Find("Canvas").transform;


        HealthObject.GetComponent<Slider>().maxValue = Health;
        HealthObject.GetComponent<Slider>().value = Health;
    }

    void Update()
    {
        HealthObject.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(10f, 30f, 0f);
        HealthObject.name = string.Format(PhotonNetwork.playerName + " HealthSlider");

        if (Health <= 0)
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

    public void AddAttributes(string nameProp, float value)
    {
        float attributeValue = (float)typeof(PlayerAttributes).GetProperty(nameProp).GetValue(this, null);
        attributeValue += value;
        typeof(PlayerAttributes).GetProperty(nameProp).SetValue(this, attributeValue, null);

        HealthObject.GetComponent<Slider>().maxValue = Health;
        HealthObject.GetComponent<Slider>().value = Health;
    }

    public void TakeDamage(float damage)
    {
        GetComponent<PhotonView>().RPC("DamageTake", PhotonTargets.AllBuffered, damage);
    }

    [PunRPC]
    public void DamageTake(float damage)
    {
        Damage = damage;
        Health -= Damage;

        HealthObject.GetComponent<Slider>().value = Health;
    }
}


