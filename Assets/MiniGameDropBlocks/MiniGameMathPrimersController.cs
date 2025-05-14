
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMathPrimersController : MonoBehaviour
{
    [SerializeField]private Material[] _materials;

    [SerializeField] private TMP_Text _textForZadanie;

    [SerializeField] private GameObject[] _otvetBlocks;

    [SerializeField] private Slider _sliderTime;

    [SerializeField] private Transform _parentToMoveFakeOtvets;

    [SerializeField] private float _timeToWaitBeforeRound;

    [SerializeField] private float _speedValueTimeSlider;

    [SerializeField] private float _speedUpKillingBox;
    [SerializeField] private float _speedDownKillingBox;

    private int _minValue = 1;
    private int _maxValue = 15;

    private int _pravilnijOtvet;

    private int[] _fakeOtvets = new int[8];

    private MeshRenderer[] _meshRendererForRandomColor = new MeshRenderer[9];
    private TMP_Text[] _otvets = new TMP_Text[9];
    private Transform[] _ObjsToMove = new Transform[9];

    private int _dontKilledIDBlock;


    [SerializeField] private float _chanceStupidPlayer;

    void Start()
    {

        for (int i=0; i<_otvetBlocks.Length;i++)
        {
            _meshRendererForRandomColor[i] = _otvetBlocks[i].GetComponent<MeshRenderer>();
            _otvets[i] = _otvetBlocks[i].GetComponentInChildren<TMP_Text>();

            _ObjsToMove[i] = _otvetBlocks[i].GetComponentInParent<BoxCollider>().transform;
        }

        MiniGameGeneral();
    }


    private void AddHard()
    {
        _maxValue += 10;

        _chanceStupidPlayer += 5;

        _speedValueTimeSlider += 2;
    }


    private void HideShowBlocksOtvets(bool _valueVisible)
    {
        foreach (GameObject _obj in _otvetBlocks)
        {
            _obj.SetActive(_valueVisible);
        }

        if(_valueVisible == true)
        {
            _sliderTime.value = 100;
        }

        _sliderTime.gameObject.SetActive(_valueVisible);

        _otvetBlocks[_dontKilledIDBlock].SetActive(true);
    }

    private void MiniGameGeneral()
    {
        HideShowBlocksOtvets(true);

        ResetGeneralPrimer();
        SetReshenieInGame();

        StartCoroutine(SliderCor());
    }

    private IEnumerator SliderCor()
    {
        while (_sliderTime.value > 0)
        {
            _sliderTime.value -= _speedValueTimeSlider * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
        HideShowBlocksOtvets(false);

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(MoveUpKiller());
    }

    private IEnumerator MoveUpKiller()
    {
        for (int i = 0; i < _ObjsToMove.Length;i++)
        {
            if (i != _dontKilledIDBlock)
            {
                _ObjsToMove[i].parent = _parentToMoveFakeOtvets;
            }
        }
        while (_parentToMoveFakeOtvets.position.y < 2.3f)
        {
            _parentToMoveFakeOtvets.transform.position += Vector3.up * _speedUpKillingBox * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1);

        StartCoroutine(MoveDownKiller());
    }
    private IEnumerator MoveDownKiller()
    {
        while (_parentToMoveFakeOtvets.position.y > -1f)
        {
            _parentToMoveFakeOtvets.transform.position -= Vector3.up * _speedDownKillingBox * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        _parentToMoveFakeOtvets.position = new Vector3(_parentToMoveFakeOtvets.position.x, -1, _parentToMoveFakeOtvets.position.z);

        for (int i = 0; i < _ObjsToMove.Length; i++)
        {
            _ObjsToMove[i].parent = null;
        }

        HideShowBlocksOtvets(false);

        yield return new WaitForSeconds(_timeToWaitBeforeRound);

        AddHard();

        MiniGameGeneral();
    }

    private void ResetGeneralPrimer()
    {
        int value1;
        int value2;
        if (Random.Range(0, 100) > 30)
        {
            value1 = Random.Range(_minValue, _maxValue);
            value2 = Random.Range(_minValue, _maxValue);
            if (value1 > value2)
            {
                _textForZadanie.text = value1.ToString() + " - " + value2.ToString();
                _pravilnijOtvet = value1- value2;
            }
            else
            {
                _textForZadanie.text = value1.ToString() + " + " + value2.ToString();
                _pravilnijOtvet = value1 + value2;
            }
        }
        else
        {
            value1 = Random.Range(_minValue, 9);
            value2 = Random.Range(_minValue, 9);
            if (value1 > value2)
            {
                _textForZadanie.text = value1.ToString() + " * " + value2.ToString();
                _pravilnijOtvet = value1 * value2;
            }
            else
            {
                _textForZadanie.text = (value1*value2).ToString() + " / " + value2.ToString();
                _pravilnijOtvet = value1;
            }
        }

        _fakeOtvets[0] = _pravilnijOtvet + Random.Range(-_maxValue/5, _maxValue / 5);
        _fakeOtvets[1] = _pravilnijOtvet + Random.Range(-_maxValue / 5, _maxValue / 5);
        if (Random.Range(0, 100) > 50)
        {
            _fakeOtvets[2] = _pravilnijOtvet + value1;
        }
        else
        {
            _fakeOtvets[2] = _pravilnijOtvet + value2;
        }

        _fakeOtvets[3] = int.Parse(value1.ToString() + value2.ToString());

        if (Random.Range(0, 100) > 50)
        {
            _fakeOtvets[4] = _pravilnijOtvet - value1;
        }
        else
        {
            _fakeOtvets[4] = _pravilnijOtvet - value2;
        }
        if (Random.Range(0, 100) > 50)
        {
            _fakeOtvets[5] = _pravilnijOtvet / value1;
        }
        else
        {
            _fakeOtvets[5] = _pravilnijOtvet / value2;
        }
        _fakeOtvets[6] = _pravilnijOtvet + Random.Range(-_maxValue, _maxValue);
        _fakeOtvets[7] = _pravilnijOtvet + Random.Range(-_maxValue, _maxValue);

        for(int i=0;i<_fakeOtvets.Length;i++)
        {
            if (_fakeOtvets[i] < 0)
            {
                _fakeOtvets[i] *= -1;
            }
            if (_fakeOtvets[i] == _pravilnijOtvet)
            {
                _fakeOtvets[i] += Random.Range(1,30);
            }
        }
    }

    private void SetReshenieInGame()
    {
        List<int> _notUsedOtvets = new List<int>();
        _notUsedOtvets.AddRange( _fakeOtvets );
        _notUsedOtvets.Add(_pravilnijOtvet);

        for (int i = 0; i < _otvetBlocks.Length; i++)
        {
            _meshRendererForRandomColor[i].material = _materials[Random.Range(0, _materials.Length)];

            int usedOtvet = _notUsedOtvets[Random.Range(0, _notUsedOtvets.Count)];

            _otvets[i].text = usedOtvet.ToString();

            _notUsedOtvets.Remove(usedOtvet);

            if(_pravilnijOtvet== usedOtvet)
            {
                _dontKilledIDBlock = i;
            }
        }
        
    }

}
