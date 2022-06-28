using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ObjectBase : MonoBehaviour,IPooledObject
{
    [SerializeField] private int objectCount;
    [Header("UI")]
    [SerializeField] private TextMeshPro objectCountText;
    [Header("Objects")]
    [SerializeField] private List<Transform> myObjects;
    [Header("Properties")]
    [SerializeField] private bool isUsed;
    public int ObjectCount { get => objectCount; private set { objectCount = value; } }
    public bool IsUsed { get => isUsed;private set { isUsed = value;} }

    #region PoolId
    public string PoolType { get; set; }
    public int PoolId { get; set; }
    #endregion
    #region PrivateVariables
    private float _height;
    private int _instantAnimIndex;
    private int _moveToPlayerIndex;
    private ConstantVariables _constantVariables;
    #endregion
    private void Awake()
    {
        Setup();
    }
    public void SetObjects(string objectPoolType, int objectPoolId, int stackCount)
    {
        ScaleAnim();
        myObjects = new List<Transform>();
        for (int i = 0; i <stackCount; i++)
        {
            Transform tempObject = ObjectPool.Singleton.GetObject(objectPoolType, objectPoolId).transform;
            tempObject.SetParent(transform);
            tempObject.DOScale(Vector3.one, 0.3f);
            tempObject.localPosition = Vector3.zero;
            myObjects.Add(tempObject);
        }
        myObjects.Reverse();
        ObjectInstantAnimations();
        ObjectCount = stackCount;
    }
    public void ObjectsHolderToPlayer(int count,Transform target)
    {
        if (ObjectCount > 0)
        {
            _moveToPlayerIndex = myObjects.Count - 1;
            MoveToPlayerTarget(target);

            IsUsed = true;
            ObjectCount -= count;
        }
        else
        {
            ObjectCount += count;
            if (ObjectCount==0)
            {
                IsUsed = true;
            }
        }
    }
    private void Setup()
    {
        _constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
    private void ObjectInstantAnimations()
    {
        DOVirtual.DelayedCall(0.06f, () =>
         {
             if (_instantAnimIndex < myObjects.Count)
             {
                 Transform tempObject = myObjects[_instantAnimIndex];
                 _height = ObjectComponentHandler.MeshHeight(tempObject.GetComponent<Renderer>()) * 2;
                 tempObject.DOLocalMoveY(_height * _instantAnimIndex, _constantVariables.ObjectMoveDuration);
                 _instantAnimIndex++;
                 SetText(_instantAnimIndex);

                 ObjectInstantAnimations();
             }
         });
    }
    private void MoveToPlayerTarget(Transform target)
    {
        DOVirtual.DelayedCall(0.06f, () =>
        {
            if (_moveToPlayerIndex>-1)
            {
                Transform tempObject = myObjects[_moveToPlayerIndex];
                tempObject.SetParent(target);
                if (DOTween.IsTweening(tempObject))
                {
                    DOTween.Kill(tempObject);
                }
                tempObject.DOLocalMove(Vector3.zero, _constantVariables.ObjectMoveDuration);
                myObjects.RemoveAt(_moveToPlayerIndex);
                _moveToPlayerIndex--;
                SetText(myObjects.Count);
                MoveToPlayerTarget(target);
            }
            else
            {
                if (ObjectCount==0)
                {
                    //Destroy
                }
            }
        });
    }
    private void CreateNewHolderAndReturnPool()
    {
        ObjectPool.Singleton.PutObject(PoolType, PoolId, gameObject);
        ObjectSpawnManager.Singleton.SpawnObjectAndSetPosition(-5);
        Debug.LogError("PutBackObjectHolder");
    }
    private void SetText(int amount)
    {
        if (amount > 0)
        {
            objectCountText.text = "+" + amount.ToString();
        }
        else
        {
            objectCountText.text = amount.ToString();
        }
        objectCountText.transform.localPosition = Vector3.up * (_height + 1.2f);
    }
    private void ScaleAnim()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f);
    }
}
