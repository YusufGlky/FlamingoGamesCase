using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StackedObjects : MonoBehaviour,IPooledObject
{
    private Collider _mCollider;
    private Rigidbody _mRigidbody;
    private HingeJoint _mHingeJoint;
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
        _mHingeJoint = GetComponent<HingeJoint>();
    }
    public void SetHingeJoint(Rigidbody connectedBody)
    {
        _mHingeJoint.connectedBody = connectedBody;
    }
    public void EnableHingeJoint()
    {

    }
    public void DisableHingeJoint()
    {

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
    public void MoveGroundAndReturnToPool(int direction, float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
         {
             transform.SetParent(null);
             transform.DOMove(transform.position + new Vector3(5 * direction, -5, 0), 1).OnComplete(() =>
             {
                 ObjectPool.Singleton.PutObject(PoolType, PoolId, gameObject);
             });
         });
    }
}
