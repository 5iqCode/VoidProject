using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame4MoveDownBoxes : MonoBehaviour
{
    private float _speedDropping= -500;

    private Transform _transformDroppingBox;

    private Transform _parentPointTransform;

    private void Awake()
    {
        _transformDroppingBox = GetComponentInChildren<BoxCollider>().transform;
    }

    public void SetValues(float _speedDrop,Transform parentPointTransform)
    {
        _speedDropping = -_speedDrop;
        _parentPointTransform = parentPointTransform;
    }

    private void Update()
    {
        _transformDroppingBox.localPosition += Vector3.up * _speedDropping * Time.deltaTime;

        if (((_transformDroppingBox.localPosition.y - 30f) < 0.1f))
        {
            _transformDroppingBox.localPosition = Vector3.up * 30f;

            _transformDroppingBox.parent = null;

            _parentPointTransform.transform.localPosition += Vector3.up * 3f;

            Destroy(this.gameObject);

        }
    }
}
