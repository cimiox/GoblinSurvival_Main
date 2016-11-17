using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class SyncronizeMovePlayer : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;

    bool firstTake = false;
    private MovePlayer _movePlayer;

    void Awake()
    {
        _movePlayer = GetComponent<MovePlayer>();
    }

    void OnEnable()
    {
        firstTake = true;
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 20);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 20);
        }
    }

    /// <summary>
    /// Interpolation move player
    /// </summary>
    /// <param name="stream"> Stream for write or read</param>
    /// <param name="info">Get inforamtion message</param>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext((int)_movePlayer._playerState);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            _movePlayer._playerState = (PlayerState)(int)stream.ReceiveNext();
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
