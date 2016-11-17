using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Linq;

public class Move : Photon.MonoBehaviour
{
    Transform PlayerTransform;
    PlayerAttributes PlayerHealth;
    UnityEngine.AI.NavMeshAgent EnemyMeshAgent;
    EnemyAttributes EnemyAttributes;

    bool firstTake = false;
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;

    public GameObject[] PlayerArray = null;
    public Dictionary<GameObject, float> PlayerDictionary;

    float timer = 0f;
    float TimeBetweenChangePlayer = 1f;

    void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerHealth = PlayerTransform.GetComponent<PlayerAttributes>();
        EnemyMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        EnemyAttributes = GetComponent<EnemyAttributes>();

        PlayerDictionary = new Dictionary<GameObject, float>();
    }

    void Start()
    {
    }

    void OnEnable()
    {
        firstTake = true;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (EnemyAttributes.enemyHealth > 0)
        {
            if (timer >= TimeBetweenChangePlayer)
            {
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
                {
                    PlayerDictionary[GameObject.FindGameObjectsWithTag("Player")[i]] = Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Player")[i].transform.position);
                }

                var localvalue = PlayerDictionary.Min(item => item.Value);

                foreach (var item in PlayerDictionary.Where(item => item.Value == localvalue).Select(item => item.Key))
                {
                    EnemyMeshAgent.SetDestination(item.transform.position);
                }
            }
        }
        else
        {
            EnemyMeshAgent.enabled = false;
        }


        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 20);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 20);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();

            if (firstTake)
            {
                firstTake = false;
                transform.position = correctPlayerPos;
                transform.rotation = correctPlayerRot;
            }
        }
    }
}
