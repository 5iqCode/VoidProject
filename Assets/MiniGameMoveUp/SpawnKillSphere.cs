using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKillSphere : MonoBehaviour
{
    [SerializeField] private GameObject _killSphere;

    private Coroutine _spawnerCoroutine;

    private float _timeToWait = 0.25f;

    private void Start()
    {
        _spawnerCoroutine = StartCoroutine(Spawner(50));
    }


    private IEnumerator Spawner(int _countSpawns)
    {
        for(int i=0;i< _countSpawns;i++)
        {
            GameObject _tempObj = Instantiate(_killSphere);

            _tempObj.transform.position = transform.position;

            _tempObj.transform.localPosition += Vector3.left* Random.Range(-20,20);

            float localScale;

            if (Random.Range(0, 100) > 80)
            {
                localScale = Random.Range(6f, 10f);
            }
            else
            {
                localScale = Random.Range(0.5f, 4f);
            }

            _tempObj.transform.localScale = Vector3.one * localScale;

            Rigidbody _rbTempObj = _tempObj.GetComponent<Rigidbody>();

            _rbTempObj.mass *= localScale;

            _rbTempObj.AddForce(new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.75f, 0.15f), Random.Range(-0.75f ,- 0.25f))* Random.Range(10000, 50000)* localScale);

            yield return new WaitForSeconds(_timeToWait);
        }

        ChangeTimeToSpawn();
    }

    private void ChangeTimeToSpawn()
    {
        if(_timeToWait == 0.15f)
        {
            _timeToWait = 0.25f;

            _spawnerCoroutine = StartCoroutine(Spawner(Random.Range(10, 30)));
        }
        else
        {
            _timeToWait = 0.15f;

            _spawnerCoroutine = StartCoroutine(Spawner(Random.Range(50, 100)));
        }
    }
}
