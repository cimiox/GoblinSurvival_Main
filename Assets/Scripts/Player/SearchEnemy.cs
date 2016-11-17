using UnityEngine;
using System.Collections;

public class SearchEnemy : MonoBehaviour
{
    private ConnectionLobby conLobbyComponent;
    public bool CheckCollider = false;
    public GameObject objectToRotate;

    void Start()
    {
        conLobbyComponent = GameObject.Find("ConnectionLobby").GetComponent<ConnectionLobby>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            objectToRotate = other.gameObject;
            if (objectToRotate.GetComponentInParent<Attack>())
            {
                CheckCollider = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            CheckCollider = false;
            objectToRotate = null;
        }
    }

    public void RotateToEnemy(float speed)
    {
        if (!CheckCollider)
            return;

        if (objectToRotate == null)
            return;

        gameObject.transform.parent.transform.LookAt(objectToRotate.GetComponentInParent<Attack>().transform.position);
        
    }
}
