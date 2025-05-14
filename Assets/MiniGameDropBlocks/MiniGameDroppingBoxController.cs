
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameDroppingBoxController : MonoBehaviour
{
    [SerializeField] private GameObject[] _boxes;

    private MeshRenderer[] _meshes;

   [SerializeField]  private float _timeToWait;

    [SerializeField] private float _chanceDrops;

    [SerializeField] private float _speedDropping;


    [SerializeField] private float _downPositionBoxes;
    [SerializeField] private float _upPositionBoxes;

    private List<int> _dontDroppingList;
    private List<int> _droppingList;


    [SerializeField] private float _chanceStupidPlayer;



    private Coroutine _job;

    private int _targetRounds = 15, _countRounds = 0;

    [SerializeField] private GameObject _winRoundWindow;

    void Start()
    {
        _dontDroppingList = new List<int>();
        _droppingList = new List<int>();



        _meshes = new MeshRenderer[_boxes.Length];

        for (int i = 0; i < _boxes.Length; i++)
        {
            _meshes[i] = _boxes[i].GetComponent<MeshRenderer>();
        }

        _job = StartCoroutine(WaitToStartMiniGame());
    }

    private void SelectMovedBoxes()
    {
        for (int i=0;i< _boxes.Length;i++)
        {

            if(Random.Range(0,101)< _chanceDrops)
            {
                _droppingList.Add(i);
                if (!_meshes[i].enabled)
                {
                    _meshes[i].enabled = true;
                }
            }
            else
            {
                if (_meshes[i].enabled)
                {
                    _meshes[i].enabled = false;
                }
                _dontDroppingList.Add(i);
            }
        }

        while (_dontDroppingList.Count < 3)
        {
            int idBox = Random.Range(0, _boxes.Length);

            if (_meshes[idBox].enabled)
            {
                _meshes[idBox].enabled = false;
            }
            _droppingList.Remove(idBox);
            _dontDroppingList.Add(idBox);
        }
        while (_droppingList.Count < 5)
        {
            int idBox = Random.Range(0, _boxes.Length);

            if (!_meshes[idBox].enabled)
            {
                _meshes[idBox].enabled = true;
            }
            _droppingList.Add(idBox);
            _dontDroppingList.Remove(idBox);
        }
    }

    private void AddMoveToBoxes()
    {
        foreach(int idDroppingBox in _droppingList)
        {
            MiniGameDropBoxesMoveDown _moveScript = _boxes[idDroppingBox].AddComponent<MiniGameDropBoxesMoveDown>();

            _moveScript._targetMovePos = _downPositionBoxes;

            _moveScript._speedDropping = -_speedDropping;
        }
    }

    private void AddMoveToBoxesUp()
    {
        foreach (int idDroppingBox in _droppingList)
        {
            MiniGameDropBoxesMoveDown _moveScript = _boxes[idDroppingBox].AddComponent<MiniGameDropBoxesMoveDown>();

            _moveScript._targetMovePos = _upPositionBoxes;

            _moveScript._speedDropping = _speedDropping/2;
        }
    }

    private void AddHard()
    {
        _timeToWait -= 0.1f;

        _chanceDrops += 5;

        _speedDropping += 10;

        foreach (int idDroppingBox in _droppingList)
        {
            _meshes[idDroppingBox].enabled = false;
        }

        _dontDroppingList.Clear();
        _droppingList.Clear();
    }

    private IEnumerator WaitToStartMiniGame()
    {
        yield return new WaitForSeconds(1);

        _job = StartCoroutine(WaitToMoveBlocks());
    }

    private IEnumerator WaitToMoveBlocks()
    {
        SelectMovedBoxes();


        yield return new WaitForSeconds(_timeToWait);

        AddMoveToBoxes();

        _job = StartCoroutine(WaitToDropBox());
    }


    private IEnumerator WaitToDropBox()
    {
        while (_boxes[_droppingList[0]].GetComponent<MiniGameDropBoxesMoveDown>())
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(_timeToWait/2);

        AddMoveToBoxesUp();

        _job = StartCoroutine(WaitToUpBox());
    }

    private IEnumerator WaitToUpBox()
    {
        while (_boxes[_droppingList[0]].GetComponent<MiniGameDropBoxesMoveDown>())
        {
            yield return new WaitForSeconds(0.1f);
        }

        AddHard();

        yield return new WaitForSeconds(_timeToWait / 2);
        
    }
}
