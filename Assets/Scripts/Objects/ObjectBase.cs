using DG.Tweening;
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
        _instantAnimIndex = 0;

        if (stackCount > 0)
        {
            for (int i = 0; i < stackCount; i++)
            {
                Transform tempObject = ObjectPool.Singleton.GetObject(objectPoolType, objectPoolId).transform;
                tempObject.GetComponent<StackedObjects>().DisableComponents();
                tempObject.SetParent(transform);
                tempObject.DOScale(Vector3.one, 0.3f);
                tempObject.localPosition = Vector3.zero;
                myObjects.Add(tempObject);
            }
            myObjects.Reverse();
            ObjectInstantAnimations();
        }
        else
        {
            SetText(stackCount);
        }
        IsUsed = false;
        ObjectCount = stackCount;
    }
    public void ObjectsHolderToPlayer(int count, Transform target)
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
            DOVirtual.Int(ObjectCount, ObjectCount - count, _constantVariables.ObjectMinusAnimDuration, x =>
                 {
                     ObjectCount = x;
                     SetText(x);

                     if (ObjectCount == 0&&!IsUsed)
                     {
                         IsUsed = true;
                         CreateNewHolderAndReturnPool();
                     }
                 });
        }
    }
    private void Setup()
    {
        _constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
    private void ObjectInstantAnimations()
    {
        DOVirtual.DelayedCall(_constantVariables.ObjectMoveDelay, () =>
         {
             if (_instantAnimIndex < myObjects.Count)
             {
                 Transform tempObject = myObjects[_instantAnimIndex];
                 if (DOTween.IsTweening(tempObject.localPosition))
                 {
                     DOTween.Kill(tempObject.localPosition);
                 }
                 _height = ObjectComponentHandler.MeshHeight(tempObject.GetComponent<Renderer>())* _instantAnimIndex;
                 tempObject.DOLocalMoveY(_height, _constantVariables.ObjectMoveDuration);
                 _instantAnimIndex++;
                 SetText(_instantAnimIndex);

                 ObjectInstantAnimations();
             }
         });
    }
    private void MoveToPlayerTarget(Transform target)
    {
        DOVirtual.DelayedCall(_constantVariables.ObjectMoveDelay, () =>
        {
            if (_moveToPlayerIndex>-1)
            {
                Transform tempObject = myObjects[_moveToPlayerIndex];
                SetObject(tempObject, target);

                myObjects.RemoveAt(_moveToPlayerIndex);

                _moveToPlayerIndex--;
                _height -= ObjectComponentHandler.MeshHeight(tempObject.GetComponent<Renderer>());

                SetText(myObjects.Count);
                MoveToPlayerTarget(target);
                ObjectManager.Singleton.MoveFinisher();
            }
            else
            {
                if (ObjectCount==0)
                {
                    CreateNewHolderAndReturnPool();
                    //Destroy
                }
            }
        });
    }
    private void SetObject(Transform tempObject,Transform target)
    {
        if (DOTween.IsTweening(tempObject))
        {
            DOTween.Kill(tempObject);
        }

        tempObject.SetParent(target);
        tempObject.DOScale(tempObject.localScale / 2, _constantVariables.ObjectMoveDuration);
        tempObject.DOLocalMove(Vector3.zero, _constantVariables.ObjectMoveDuration);
    }
    private void CreateNewHolderAndReturnPool()
    {
        ObjectPool.Singleton.PutObject(PoolType, PoolId, gameObject);
        ObjectManager.Singleton.SpawnNewObjectHolder();
    }
    private void SetText(int amount)
    {
        if (amount > 0)
        {
            objectCountText.text = "+" + amount.ToString();
            objectCountText.DOColor(new Color(0,0.7f,1,1), 0.1f);
        }
        else
        {
            objectCountText.text = amount.ToString();
            objectCountText.DOColor(Color.red, 0.1f);
        }
    }
    private void ScaleAnim()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f);
    }
}
