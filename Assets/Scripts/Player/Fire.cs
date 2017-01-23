using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityStandardAssets.CrossPlatformInput;

public class Fire : Photon.MonoBehaviour
{
    public GameObject ConnectionLobby;
    PlayerAttributes Player;
    int shootableMask;
    Ray shootRay;
    RaycastHit shootHit;
    LineRenderer gunLine;
    float range = 50f;

    bool CheckArray;

    public GameObject EnemyMinRange;

    private Collider[] SphereCollider;
    public Dictionary<GameObject, float> EnemyInRangeArray;

    public GameObject PlayerRotate;
    public GameObject EnemyWhereTurn;

    public Animator Animator;

    void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        Player = GetComponent<PlayerAttributes>();
        ConnectionLobby = GameObject.Find("ConnectionLobby");
        Animator = GetComponentInParent<Animator>();
        EnemyInRangeArray = new Dictionary<GameObject, float>();
    }

    public void Shoot()
    {
        if (photonView.isMine)
        {
            gunLine.enabled = true;
            gunLine.SetPosition(0, transform.position);

            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            if (Physics.Raycast(shootRay, out shootHit, 10f, shootableMask))
            {
                if (shootHit.collider.GetComponent<EnemyAttributes>())
                {
                    shootHit.collider.GetComponent<EnemyAttributes>().TakeDamage(Random.RandomRange(1, 100));
                }
                else if (shootHit.collider.GetComponentInParent<EnemyAttributes>())
                {
                    shootHit.collider.GetComponentInParent<EnemyAttributes>().TakeDamage(Random.RandomRange(1, 100));
                }

                gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * 10f);
            }

            EnemyInRangeArray.Clear();
        }
    }

    public void DisableShoot()
    {
        gunLine.enabled = false;
    }

    private void FindObjectssInRange()
    {
        CheckArray = false;
        SphereCollider = Physics.OverlapSphere(transform.position, 20f);

        if (SphereCollider.Length == 0 || SphereCollider == null)
            return;

        foreach (var item in SphereCollider)
        {
            if (item.gameObject.CompareTag("Enemy"))
            {
                EnemyInRangeArray[item.gameObject] = Vector3.Distance(transform.position, item.gameObject.transform.position);
                PlayerRotate = null;
                EnemyWhereTurn = null;
                CheckArray = true;
            }
        }

        if (!CheckArray)
            return;


        var MinItem = EnemyInRangeArray.Min(item => item.Value);

        foreach (var item in EnemyInRangeArray.Where(item => item.Value == MinItem).Select(item => item.Key))
        {
            PlayerRotate = gameObject.GetComponentInParent<PlayerAttributes>().gameObject;
            EnemyWhereTurn = item.GetComponentInParent<EnemyAttributes>().gameObject;
        }
    }
}
