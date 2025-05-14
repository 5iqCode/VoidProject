
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame5Controller : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    int _countRounds = 0;

    private MeshRenderer[] _plitki;

    private int _targetCountColor = 2;

    private int _dontDropIdMat;

    private bool isDance = true;

    [SerializeField] private MeshRenderer _dontDropColorVisualizator;


    [SerializeField] private GameObject _bubbiDance;
    private AudioSource _audioBubbiDance;

  [SerializeField]  private int _chanceStupidPlayer;

    private void Start()
    {
        _bubbiDance.SetActive(false);
        _plitki = GetComponentsInChildren<MeshRenderer>();

        _audioBubbiDance = _bubbiDance.GetComponent<AudioSource>();

        StartCoroutine(GameManagerCoroutine());
    }


    private IEnumerator GameManagerCoroutine()
    {
        while(true)
        {
            _countRounds++;
            StartCoroutine(RandomPlitki());

            _dontDropIdMat = Random.Range(0, _targetCountColor);

            _dontDropColorVisualizator.material = _materials[_dontDropIdMat];

            _bubbiDance.SetActive(true);
            _audioBubbiDance.Play();
            

            yield return new WaitForSeconds(Random.Range(1, 5));

            _audioBubbiDance.Stop();
            _bubbiDance.SetActive(false);

            isDance = false;

            yield return new WaitForSeconds(6);

            isDance = true;

            if (_countRounds % 2 == 0)
            {
                if (_targetCountColor < _materials.Length)
                {
                    _targetCountColor++;
                }
            }
        }
    }


    private IEnumerator RandomPlitki()
    {
        while (isDance)
        {
            foreach (MeshRenderer _plitka in _plitki)
            {
                int idMat = Random.Range(0, _targetCountColor);

                _plitka.material = _materials[idMat];

                _plitka.name = idMat.ToString();
            }
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(CorountineDropPlitki());
    }

    private IEnumerator CorountineDropPlitki()
    {

        List<Transform> _safePlitka = new List<Transform> ();

        foreach (MeshRenderer _plitka in _plitki)
        {
            if (_plitka.name == _dontDropIdMat.ToString())
            {
                _safePlitka.Add(_plitka.transform);
            }
        }
        if (_safePlitka.Count<5)
        {
            _safePlitka.AddRange(AddPlitki(5 - _safePlitka.Count));
        }

        Transform[] _safedPointMas = new Transform[_safePlitka.Count];

        for(int i = 0; i < _safePlitka.Count; i++)
        {
            _safedPointMas[i] = _safePlitka[i];
        }


        yield return new WaitForSeconds(3);

        foreach (MeshRenderer _plitka in _plitki)
        {
            if (_plitka.name != _dontDropIdMat.ToString())
            {
                _plitka.gameObject.SetActive(false);
            }
        }

        yield return new WaitForSeconds(2);
        foreach (MeshRenderer _plitka in _plitki)
        {
            if (_plitka.name != _dontDropIdMat.ToString())
            {
                _plitka.gameObject.SetActive(true);
            }
        }
    }


    private Transform[] AddPlitki(int needAdd)
    {
        Transform[] _addPlitki = new Transform[needAdd];
        for(int i=0;i< needAdd; i++)
        {
            MeshRenderer _tempPlitka = _plitki[Random.Range(0, _plitki.Length)];

            _tempPlitka.material = _materials[_dontDropIdMat];

            _tempPlitka.name = _dontDropIdMat.ToString();

            _addPlitki[i] = _tempPlitka.transform;
        }

        return _addPlitki;
    }

}
