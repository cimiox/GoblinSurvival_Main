using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class ConnectionLobby : Photon.MonoBehaviour
{
    public static ConnectionLobby Instance = null;

    public GameObject NewPlayer { get; set; }
    public GameObject HealthPrefab;
    public bool AutoConnect = true;
    public LookAtPlayer TargetPlayer;
    public GameObject Spawn;
    public Fire PlayerFireComponent;
    public int countEnemy = 5;
    public int CountEnemyDeath { get; set; }

    public byte Version = 1;
    float timer = 0;
    float TimeBetweenBullets = 1f;
    public bool isShoot = false;

    private bool ConnectInUpdate = true;

    bool Check = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        Spawn = GameObject.Find("Spawns");
        StartCoroutine(NewEnemy(countEnemy));
    }


    public virtual void FixedUpdate()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }

        timer += Time.deltaTime;

        if (isShoot && timer >= TimeBetweenBullets)
        {
            PlayerFireComponent.Shoot();
            timer = 0;
        }

        if (timer >= TimeBetweenBullets / 10f)
        {
            PlayerFireComponent.DisableShoot();
        }

        if (CountEnemyDeath == countEnemy)
        {
            Manager.Instance.CountEnemyDeath = this.CountEnemyDeath;
            ReturnToLobby();
        }
    }

    /// <summary>
    /// Action after join in room
    /// </summary>
    void OnJoinedRoom()
    {
        Vector3 position = new Vector3(-3.2f, 1.5f, -0.9f);
        GameObject newPlayer = PhotonNetwork.Instantiate("Goblin", position, Quaternion.identity, 0);
        TargetPlayer.target = newPlayer.transform;
        NewPlayer = newPlayer;
        PlayerFireComponent = NewPlayer.GetComponentInChildren<Fire>();
        newPlayer.gameObject.name = string.Format("Goblin {0}", Random.Range(1, 100));
    }

    IEnumerator NewEnemy(int enemyCount)
    {
        while (enemyCount >= 1)
        {
            yield return new WaitForSeconds(1f);
            if (PhotonNetwork.isMasterClient)
            {
                GameObject newEnemy = PhotonNetwork.InstantiateSceneObject("Spider", SpawnEnemy(Random.Range(1, 3)), Quaternion.identity, 0, null);
            }
            enemyCount--;
            yield return new WaitForSeconds(4f);
        }
    }

    public void ShootGameObject()
    {
        isShoot = true;
    }

    public void StopShoot()
    {
        isShoot = false;
    }

    public void ReturnToLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnDisconnectedFromPhoton()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public Vector3 SpawnEnemy(int numberSpawn)
    {
        switch (numberSpawn)
        {
            case 1:
            FirstCase:
                return Spawn.transform.GetChild(0).position;
            case 2:
                return Spawn.transform.GetChild(1).position;
            case 3:
                return Spawn.transform.GetChild(2).position;
            default:
                goto FirstCase;
        }
    }
}
