using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBlockMiniGame3 : MonoBehaviour
{
    private float _speed = 100;

    private Material _customMaterial;

    private GameObject jumpTriggerPrefab;

    private void Start()
    {
        _customMaterial = new Material(Shader.Find("Standard"));

        MeshRenderer _meshRenderer = GetComponent<MeshRenderer>();

        _customMaterial.CopyMatchingPropertiesFromMaterial(_meshRenderer.material);
        _meshRenderer.material = _customMaterial;
    }
    

    public void DropBlock(float zadershkaTime,GameObject _jumpTrigger)
    {
        jumpTriggerPrefab = _jumpTrigger;

        _speed /= zadershkaTime;

        StartCoroutine(WaitToDrop(zadershkaTime * 3)); 
    }

    

    private IEnumerator WaitToDrop(float _time)
    {
        float currentTime = 0;

        float _zValue =0;

        int _direction = 1;
        Color _color = Color.white;

        while (currentTime< _time)
        {
            float _deltaTime = Time.fixedDeltaTime;

            currentTime+= _deltaTime;

            _color.g -= _deltaTime * _speed/250;
            _color.b -= _deltaTime * _speed / 250;
            GetComponent<MeshRenderer>().material.color = _color;

            _speed += _deltaTime*2;

            

            if (_zValue > 5)
            {
                _direction = -1;
            }else if (_zValue < -5)
            {
                _direction = 1;
            }

            _zValue += _deltaTime * _speed * _direction;

            transform.rotation = Quaternion.Euler(Vector3.forward * _zValue);

            yield return new WaitForFixedUpdate();
        }
        gameObject.transform.right = Vector3.zero;

        GameObject _jumpTrigger = Instantiate(jumpTriggerPrefab, transform);
        _jumpTrigger.transform.parent = null;

        transform.localScale = new Vector3(4.9f,0.2f, 4.9f);
        gameObject.AddComponent<Rigidbody>();

        yield return new WaitForSeconds(1f);
        
        GameObject.Find("MiniGame3Controller").GetComponent<MiniGame3Controller>().UpdateNavMeshSurface();

        Destroy(gameObject, 1);
    }
}
