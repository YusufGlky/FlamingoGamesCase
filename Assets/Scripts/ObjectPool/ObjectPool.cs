using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ObjectPool : MonoSingleton<ObjectPool>
{
    [Serializable]
    public class PoolTypes
    {
        public string PoolTypeName;
        public Pool[] TypedPool;
        public PoolTypes(string _poolTypeName, Pool[] _typedPool)
        {
            PoolTypeName = _poolTypeName;
            TypedPool = _typedPool;
        }
    }
    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> PooledObjects;
        public GameObject ObjectPrefab;
        public int PoolSize;
    }
    [SerializeField] private PoolTypes[] pools = null;
    private void Awake()
    {
        SpawnObject();
    }
    private void SpawnObject()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            for (int y = 0; y < pools[i].TypedPool.Length; y++)
            {
                pools[i].TypedPool[y].PooledObjects = new Queue<GameObject>();
                GameObject g = new GameObject(pools[i].PoolTypeName);
                g.transform.SetParent(transform);
                for (int j = 0; j < pools[i].TypedPool[y].PoolSize; j++)
                {
                    GameObject obj = Instantiate(pools[i].TypedPool[y].ObjectPrefab, g.transform);
                    obj.SetActive(false);
                    obj.GetComponent<IPooledObject>().PoolType = pools[i].PoolTypeName;
                    obj.GetComponent<IPooledObject>().PoolId = y;
                    pools[i].TypedPool[y].PooledObjects.Enqueue(obj);
                }
            }
        }
    }
    public bool PoolObjectExist(string _poolType, int _poolId = 0)
    {
        bool isExist = pools.Where(x => x.PoolTypeName == _poolType).FirstOrDefault().TypedPool[_poolId].PooledObjects.Count > 0;
        return isExist;
    }
    public GameObject GetObject(string _poolType, int _poolId = 0)
    {
        GameObject lastObj = null;
        int _newPoolId = 0;
        if (_poolId == 0)
        {
            _newPoolId = UnityEngine.Random.Range(0, pools.Where(x => x.PoolTypeName == _poolType).FirstOrDefault().TypedPool.Length - 1);
        }
        else
        {
            _newPoolId = _poolId;
        }
        lastObj = pools.Where(x => x.PoolTypeName == _poolType).FirstOrDefault().TypedPool[_newPoolId].PooledObjects.Dequeue();
        lastObj.SetActive(true);
        return lastObj;
    }
    public void PutObject(string _poolType, int _poolId, object _any)
    {
        GameObject obj = (GameObject)_any;
        obj.transform.SetParent(transform);
        obj.gameObject.SetActive(false);
        pools.Where(x => x.PoolTypeName == _poolType).FirstOrDefault().TypedPool[_poolId].PooledObjects.Enqueue(obj);
    }
}
