using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Rotate : MonoBehaviour
{
    public Animation _animation;
    public AnimationClip AnimationAll;

    void Start()
    {
        _animation[AnimationAll.name].speed = 0.85f;
        _animation[AnimationAll.name].wrapMode = WrapMode.Loop;
        _animation.CrossFade(AnimationAll.name);
    }

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * 200f * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, - rotX);
    }
}
