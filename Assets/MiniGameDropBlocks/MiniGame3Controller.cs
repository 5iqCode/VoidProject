
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class MiniGame3Controller : MonoBehaviour
{
    private List<Transform> _safedPointsTransform;

    private NavMeshSurface _meshSurface;

   [SerializeField] private GameObject _jumpTrigger;


    private void Awake()
    {
        _meshSurface = GameObject.Find("NavMeshSurface").GetComponent<NavMeshSurface>();

        _meshSurface.BuildNavMesh();


        _safedPointsTransform = new List<Transform>();

        GameObject[] _droppingBlocks = GameObject.FindGameObjectsWithTag("DroppingBlock");

        foreach (var block in _droppingBlocks)
        {
            _safedPointsTransform.Add(block.transform);
        }

        StartCoroutine(GeneralCoroutine());
    }

    public Transform[] GetSavePoints()
    {
        return _safedPointsTransform.ToArray();
    }

    public void UpdateNavMeshSurface()
    {
        _meshSurface.BuildNavMesh();
    }

    private IEnumerator GeneralCoroutine()
    {
        while (_safedPointsTransform.Count>3)
        {
            yield return new WaitForSeconds(Random.Range(1f,2f));

            UpdateMap(Random.Range(1,4));

            yield return new WaitForSeconds(0.3f);
        }
    }



    private void UpdateMap(int _countDroppingBlocks)
    {
        for(int i =0;i<_countDroppingBlocks;i++)
        {
            int _droppingId = Random.Range(0, _safedPointsTransform.Count);

            _safedPointsTransform.Remove(_safedPointsTransform[_droppingId]);


        }
    }
}
