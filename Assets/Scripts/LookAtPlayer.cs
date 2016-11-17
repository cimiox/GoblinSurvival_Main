using UnityEngine;
using System.Collections;

/// <summary>
/// Camera look at player
/// </summary>
public class LookAtPlayer : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null)
            return;

        transform.position = new Vector3(target.transform.position.x - 20, target.transform.position.y + 50, target.transform.position.z);
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
    }
}
