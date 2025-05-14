using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameDropBoxesMoveDown : MonoBehaviour
{
    public float _speedDropping;

    public float _targetMovePos;

    private Transform _transformDroppingBox;

    private float _defoultPositionUp = 34.499f;


    private void Awake()
    {
        _transformDroppingBox = GetComponentInChildren<BoxCollider>().transform;
    }

    private void Update()
    {
        _transformDroppingBox.localPosition += Vector3.up * _speedDropping * Time.deltaTime;

        if (((_transformDroppingBox.localPosition.y - _targetMovePos)< 0.1f)&& (_speedDropping<0))
        {
            Destroy(this);

        } else if(((_targetMovePos - _transformDroppingBox.localPosition.y) < 0.1f) && (_speedDropping > 0))
        {
            _transformDroppingBox.localPosition = Vector3.up * _defoultPositionUp;
            Destroy(this);
        }
    }
}
