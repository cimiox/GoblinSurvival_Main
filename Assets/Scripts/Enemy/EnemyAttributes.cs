using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyAttributes : Photon.MonoBehaviour
{
    public float maxEnemyHealth = 100;
    public float enemyHealth = 0;
    public float enemyDamage;
    GameObject HealthObject;
    int firstTakeDamage = 0;
    int countSliders = 0;
    private GameObject Canvas;
    private ConnectionLobby _conLobby;

    void Awake()
    {
        _conLobby = GameObject.Find("ConnectionLobby").GetComponent<ConnectionLobby>();
        enemyHealth = maxEnemyHealth;
        Canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        if (firstTakeDamage >= 1)
        {
            HealthObject.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(10f, 30f, 0f);
            HealthObject.name = string.Format("EnemyHealthSlider" + countSliders);
        }
    }

    public void TakeDamage(float Damage)
    {
        do
        {
            firstTakeDamage++;
        } while (firstTakeDamage == 10);

        if (firstTakeDamage == 1)
        {
            HealthObject = PhotonNetwork.Instantiate("HealthSlider", Vector3.one, Quaternion.identity, 0) as GameObject;
            HealthObject.transform.parent = Canvas.transform;

            HealthObject.GetComponent<Slider>().maxValue = maxEnemyHealth;
            HealthObject.GetComponent<Slider>().value = enemyHealth;
        }

        enemyHealth -= Damage;
        HealthObject.GetComponent<Slider>().value = enemyHealth;

        if (enemyHealth <= 0)
        {
            _conLobby.CountEnemyDeath++;
            PhotonNetwork.Destroy(photonView);
            PhotonNetwork.Destroy(HealthObject);
        }
    }
}
