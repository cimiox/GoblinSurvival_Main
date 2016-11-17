using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    GameObject Player;
    EnemyAttributes EnemyAtt;
    PlayerAttributes PlayerAtt;

    float timer = 0.0f;
    float TimeBetweenAttack = 1f;

    bool PlayerInRange;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerAtt = Player.GetComponent<PlayerAttributes>();
        EnemyAtt = GetComponent<EnemyAttributes>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            PlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            PlayerInRange = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= TimeBetweenAttack && PlayerInRange && EnemyAtt.enemyHealth > 0)
        {
            EnemyAttack();
        }
    }

    public void EnemyAttack()
    {
        timer = 0f;

        if (PlayerAtt.HealthPlayer > 0)
        {
            PlayerAtt.TakeDamage(EnemyAtt.enemyDamage);
        }
    }
}
