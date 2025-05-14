
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MiniGame4Controller : MonoBehaviour
{
    [SerializeField] private GameObject DamageBoxPrefab;

    [SerializeField] private Transform[] _points;

    private NavMeshSurface _meshSurface;

    private int _speedDrop = 500;

    private int countSpawnBoxes = 5;

    private int countRounds = 0;

    private float _timeBetweenRounds = 2;

    private float _timeZadershki = 5;

    private Transform[] _safedPoints;
    private void Awake()
    {
        _meshSurface = GameObject.Find("NavMeshSurface").GetComponent<NavMeshSurface>();

        _meshSurface.BuildNavMesh();

        GameObject[] _playersAI = GameObject.FindGameObjectsWithTag("PlayerAI");

        StartCoroutine(GeneralCor());
    }

    private IEnumerator GeneralCor()
    {
        yield return new WaitForSeconds(_timeBetweenRounds);

        _meshSurface.BuildNavMesh();

        StartCoroutine(SpawnBoxes(countSpawnBoxes, _timeZadershki));

        countRounds++;
    }

    private void AddHard()
    {
        _speedDrop += 100;

        if (countRounds % 2 == 0)
        {
            _timeZadershki -= 0.1f;
        }
        if (countRounds % 3 == 0)
        {
            if (countSpawnBoxes < 10)
            {
                countSpawnBoxes++;
            }
        }

        if (_timeBetweenRounds > 0.5f)
        {
            _timeBetweenRounds -= 0.1f;
        }
    }


    private IEnumerator SpawnBoxes(int _countBoxes,float _timeToWait)
    {

        List<Transform> _currentSafedPoint = new List<Transform>();
        _currentSafedPoint.AddRange(_points);

        for (int i=0;i< _countBoxes; i++)
        {
            int curentId = Random.Range(0, _points.Length);

            while (_points[curentId].GetComponentInChildren<BoxCollider>() != null)
            {
                curentId = Random.Range(0, _points.Length);
            }

            _currentSafedPoint.Remove(_points[curentId]);

            GameObject _droppingBox = Instantiate(DamageBoxPrefab, _points[curentId]);

            StartCoroutine(WaitToMoveDownCor(_timeToWait, _droppingBox, _points[curentId]));
        }

        _safedPoints = new Transform[_currentSafedPoint.Count];


        for(int i=0;i< _safedPoints.Length; i++)
        {
            _safedPoints[i] = _currentSafedPoint[i];
        }

        yield return new WaitForSeconds(_timeToWait+0.5f);

        AddHard();

        StartCoroutine(GeneralCor());
    }


    private IEnumerator WaitToMoveDownCor(float _waitSeconds, GameObject _masObjs,Transform _parentPoint)
    {
        yield return new WaitForSeconds(_waitSeconds);

        _masObjs.AddComponent<MiniGame4MoveDownBoxes>().SetValues(_speedDrop,_parentPoint);
    }

}
