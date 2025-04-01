using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Spawner<TBehaviour> : MonoBehaviour where TBehaviour : MonoBehaviour
{
    [SerializeField]
    TBehaviour _prefab;
    [SerializeField]
    float _intervalBetweenSpawns;
    [SerializeField]
    int _amountLimit;
    List<TBehaviour> _pool = new List<TBehaviour>();

    private void OnEnable()
    {
        StartCoroutine(SpawnCycle());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    protected virtual TBehaviour SpawnObject()
    {
        List<TBehaviour> availablePool = _pool.Where(obj=>!obj.gameObject.activeSelf).ToList();
        TBehaviour spawnedObject;
        if (availablePool.Count > 0)
        {
            spawnedObject = availablePool.First();
            spawnedObject.gameObject.SetActive(true);
        }
        else
        {
            spawnedObject = Instantiate(_prefab);
            _pool.Add(spawnedObject);
        }
        spawnedObject.transform.position = transform.position;
        return spawnedObject;
    }
    private bool CanSpawn()
    {
        return _pool.Where(obj=>obj.gameObject.activeSelf).Count()< _amountLimit;
    }
    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_intervalBetweenSpawns);
            if (CanSpawn())
            {
                SpawnObject();
            }
        }
    }
}
