using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedObjects : MonoBehaviour,IPooledObject
{
    private Collider _mCollider;
    private Rigidbody _mRigidbody;
    #region PoolId
    public string PoolType { get; set; }
    public int PoolId { get; set; }
    #endregion
    private void Awake()
    {
        Setup();   
    }
    private void Setup()
    {
        _mCollider = GetComponent<Collider>();
        _mRigidbody = GetComponent<Rigidbody>();
    }
    public void EnableComponents()
    {
        _mCollider.enabled = true;
        _mRigidbody.isKinematic = false;
        _mRigidbody.constraints = RigidbodyConstraints.None;
    }
    public void DisableComponents()
    {
        _mCollider.enabled = false;
        _mRigidbody.isKinematic = true;
        _mRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void ReturnToPool()
    {
        ObjectPool.Singleton.PutObject(PoolType, PoolId, gameObject);
    }
}
