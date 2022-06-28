using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Playerbase : MonoBehaviour
{
    [SerializeField] private protected bool moveable;
    [Header("Balance")]
    [SerializeField] private protected int leftStick;
    [SerializeField] private protected int rightStick;
    [SerializeField] private float balanceValue=0.5f;
    [Header("Stick")]
    [SerializeField] private List<Transform> leftStickObjects;
    [SerializeField] private List<Transform> rightStickObjects;
    [SerializeField] private Transform leftStickTransform;
    [SerializeField] private Transform rightStickTransform;
    #region ConstantVariables
    private protected PlayerValues playerValues;
    private protected ConstantVariables constantVariables;
    #endregion
    #region privateVariables
    private Vector3 _mouseStartPos;
    private Vector3 _mouseEndPos;
    #endregion
    private void Awake()
    {
        Setup();
        TestStart();
    }
    private void OnEnable()
    {
        SetActions(true); 
    }
    private void OnDisable()
    {
        SetActions(false);
    }
    private void Update()
    {
        Movement();
        ClaimSystem();
        TestUpdate();
    }
    private void Setup()
    {
        GetScriptableObjects();
    }
    private void SetActions(bool enabled)
    {
        if (enabled)
        {
            ObjectManager.MoveFinisherAction += CheckSticks;
        }
        else
        {
            ObjectManager.MoveFinisherAction -= CheckSticks;
        }
    }
    private void GetScriptableObjects()
    {
        playerValues = Resources.Load<PlayerValues>("ScriptableObjects/Player/PlayerValues");
        constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
    private void ClaimSystem()
    {
        if (ObjectManager.Singleton.CurrentObject != null)
        {
            if (!ObjectManager.Singleton.CurrentObject.IsUsed)
            {
                ClaimDirection();
            }
        }
    }
    private void ClaimObject(int direction)
    {
        int objectAmount = ObjectAmount(direction);
        if (objectAmount != 0)
        {
            if (direction == -1)
            {
                leftStick += objectAmount;
                ObjectManager.Singleton.CurrentObject.ObjectsHolderToPlayer(objectAmount, leftStickTransform);
            }
            else
            {
                rightStick += objectAmount;
                ObjectManager.Singleton.CurrentObject.ObjectsHolderToPlayer(objectAmount, rightStickTransform);
            }
            if (ObjectManager.Singleton.CurrentObject.ObjectCount < 0)
            {
                ObjectMoveToGround(objectAmount, direction);
            }
            BalanceSystem((float)objectAmount / (float)constantVariables.MaxStackableObjects, direction);
        }
    }
    private int ObjectAmount(int direction)
    {
        int givenObject=0;
        if (ObjectManager.Singleton.CurrentObject.ObjectCount >0)
        {
            givenObject = ObjectManager.Singleton.CurrentObject.ObjectCount;
        }
        else
        {
            if (direction == -1)
            {
                if (Mathf.Abs(ObjectManager.Singleton.CurrentObject.ObjectCount) >= leftStick)
                {
                    givenObject = -leftStick;
                }
                else
                {
                    givenObject = (ObjectManager.Singleton.CurrentObject.ObjectCount + leftStick) - leftStick;
                }
            }
            else if (direction == 1)
            {
                if (Mathf.Abs(ObjectManager.Singleton.CurrentObject.ObjectCount)>=rightStick)
                {
                    givenObject = -rightStick;
                }
                else
                {
                    givenObject =(ObjectManager.Singleton.CurrentObject.ObjectCount + rightStick)-rightStick;
                }
            }
        }
        Debug.LogError("GivenObject:" + givenObject);
        return givenObject;
    }
    private void Movement()
    {
        if (moveable)
        {
            transform.Translate(Vector3.forward * playerValues.PlayerSpeed*Time.deltaTime);
        }
    }
    private void ClaimDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseEndPos = Input.mousePosition;
            if (Mathf.Abs(_mouseStartPos.x - _mouseEndPos.x) > 120)
            {
                if (_mouseStartPos.x > _mouseEndPos.x)//Left
                {
                    ClaimObject(-1);
                }
                if (_mouseStartPos.x < _mouseEndPos.x)//Right
                {
                    ClaimObject(1);
                }
            }
        }
    }
    private void BalanceSystem(float value, int direction)
    {
        float newValue = (value * direction);
        BalanceValue(balanceValue + newValue,constantVariables.BalanceChangeDuration);
    }
    private void BalanceValue(float value,float changeDuration)
    {
        if (DOTween.IsTweening(balanceValue))
        {
            DOTween.Kill(balanceValue);
        }
        DOVirtual.Float(balanceValue, value, changeDuration, x =>
        {
            balanceValue = x;
            Overlay.Singleton.BalanceMeteer(balanceValue);
            if (balanceValue <= -1 || balanceValue >= 1)
            {
                Death();
            }
        });
    }
    public float test;
    private void CheckSticks()
    {
        leftStickObjects.Clear();
        rightStickObjects.Clear();
        Transform child = null;
        for (int i = 0; i < leftStickTransform.childCount; i++)
        {
            child = leftStickTransform.GetChild(i);
            child.DOLocalMoveY(0.05f * i, constantVariables.ObjectMoveDuration);
            leftStickObjects.Add(child);
        }
        for (int i = 0; i < rightStickTransform.childCount; i++)
        {
            child = rightStickTransform.GetChild(i);
            child.DOLocalMoveY(0.05f * i, constantVariables.ObjectMoveDuration);
            rightStickObjects.Add(child);
        }
    }
    private void ObjectMoveToGround(int count,int direction)
    {
        Transform child = null;
        List<Transform> removedList = new List<Transform>();
        leftStickObjects = leftStickObjects.OrderByDescending(x => x.position.y).ToList();
        rightStickObjects = rightStickObjects.OrderByDescending(x => x.position.y).ToList();
        for (int i = 0; i < Mathf.Abs(count); i++)
        {
            if (direction == -1)
            {
                child = leftStickObjects[i];
                child.GetComponent<StackedObjects>().MoveGroundAndReturnToPool(-1,constantVariables.ObjectMoveDelay*i);
            }
            else
            {
                child = rightStickObjects[i];
                child.GetComponent<StackedObjects>().MoveGroundAndReturnToPool(1, constantVariables.ObjectMoveDelay * i);
            }
            removedList.Add(child);
        }
        for (int i = 0; i < removedList.Count; i++)
        {
            leftStickObjects.Remove(removedList[i]);
        }
    }
    private void Death()
    {
        //To Do: Death
    }
    private void TestStart()
    {
        if (LevelManager.Singleton.TestStart)
        {
            moveable = true;
        }
    }
    private void TestUpdate()
    {
        if (LevelManager.Singleton.TestUpdate)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
            }
        }
        if (Input.GetKeyDown(KeyCode.A))//LeftStick
        {
        }
        if (Input.GetKeyDown(KeyCode.D))//RightStick
        {
        }
    }
}