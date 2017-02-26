using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public enum PlayerState
{
    IdleFirst = 0,
    IdleSecond = 1,
    TakeGunBack = 2,
    RunGunBack = 3,
    RunShoot = 4,
    Shoot = 5,
    Death = 6,
}

public class MovePlayer : Photon.MonoBehaviour
{
    #region Animation

    public PlayerState _playerState;

    public AnimationClip IdleFirstAnimation;
    public AnimationClip IdleSecondAnimation;
    public AnimationClip TakeGunBackAnimation;
    public AnimationClip RunGunBackAnimation;
    public AnimationClip RunShootAnimation;
    public AnimationClip ShootAnimation;
    public AnimationClip DeathAnimation;

    private Animation _animation;

    private Animator animator;
    #endregion Animation


    private float horizontal;
    public float Horizontal
    {
        get { return CrossPlatformInputManager.GetAxis("Horizontal"); }
    }

    private float vertical;
    public float Vertical
    {
        get { return CrossPlatformInputManager.GetAxis("Vertical"); }
    }

    public float speedRotate = 8f;

    private ConnectionLobby conLobbyComponent;
    private SearchEnemy searchEnemyComponent;

    float timer = 0.0f;
    bool CheckTakeGunAnimation = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        conLobbyComponent = GameObject.Find("ConnectionLobby").GetComponent<ConnectionLobby>();
        searchEnemyComponent = GetComponentInChildren<SearchEnemy>();
        _animation = GetComponent<Animation>();
    }

    void FixedUpdate()
    {
        if (photonView.isMine)
        {
            transform.position = new Vector3(transform.position.x + Vertical * Time.deltaTime * speedRotate, 0, transform.position.z - Horizontal * Time.deltaTime * speedRotate);

            if (Horizontal != 0 && Vertical != 0)
            {
                if (!conLobbyComponent.isShoot || !searchEnemyComponent.CheckCollider || searchEnemyComponent.objectToRotate == null)
                {
                    RotatePlayer();
                    //_playerState = PlayerState.TakeGunBack;
                }
                else
                {
                    searchEnemyComponent.RotateToEnemy(speedRotate);
                    //if (Vertical == 0 && Horizontal == 0)
                    //{
                    //    _playerState = PlayerState.Shoot;
                    //}
                    //else
                    //{
                    //    _playerState = PlayerState.RunShoot;
                    //}
                }
            }
            else
            {
                if (conLobbyComponent.isShoot)
                {
                    searchEnemyComponent.RotateToEnemy(speedRotate);

                    //if (Vertical == 0 && Horizontal == 0)
                    //{
                    //    _playerState = PlayerState.Shoot;
                    //}
                    //else
                    //{
                    //    _playerState = PlayerState.RunShoot;
                    //}
                }
                else
                {
                    //_playerState = PlayerState.IdleFirst;
                }
            }
        }
    }
    
    void Update()
    {
        //if (_playerState == PlayerState.IdleFirst)
        //{
        //    timer += Time.deltaTime;
        //    _animation[IdleFirstAnimation.name].wrapMode = WrapMode.ClampForever;
        //    _animation.CrossFade(IdleFirstAnimation.name);

        //    if (timer >= _animation[IdleFirstAnimation.name].clip.averageDuration)
        //    {
        //        _animation.CrossFade(IdleSecondAnimation.name);
        //        timer = 0;
        //    }
        //    CheckTakeGunAnimation = true;
        //}
        //else if (_playerState == PlayerState.TakeGunBack)
        //{
        //    if (CheckTakeGunAnimation)
        //    {
        //        timer += Time.deltaTime;
        //        _animation[TakeGunBackAnimation.name].wrapMode = WrapMode.ClampForever;
        //        _animation.CrossFade(TakeGunBackAnimation.name);
        //    }

        //    if (timer >= _animation[TakeGunBackAnimation.name].clip.averageDuration || !CheckTakeGunAnimation)
        //    {
        //        _animation[RunGunBackAnimation.name].speed = 0.8f;
        //        _animation.CrossFade(RunGunBackAnimation.name);
        //        CheckTakeGunAnimation = false;
        //    }
        //}
        //else if (_playerState == PlayerState.Shoot)
        //{
        //    _animation[ShootAnimation.name].wrapMode = WrapMode.Loop;
        //    _animation[ShootAnimation.name].speed = 0.35f;
        //    _animation.CrossFade(ShootAnimation.name);
        //}
        //else if (_playerState == PlayerState.RunShoot)
        //{
        //    _animation[RunShootAnimation.name].wrapMode = WrapMode.Loop;
        //    _animation.CrossFade(RunShootAnimation.name);
        //}
        //else
        //{
        //    _animation[DeathAnimation.name].wrapMode = WrapMode.Loop;
        //    _animation.CrossFade(DeathAnimation.name);
        //}
    }

    private void RotatePlayer()
    {
        var Atan = Mathf.Atan2(-Horizontal, -Vertical) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, Atan - 90, 0), 5f * Time.deltaTime);
    }
}
