using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Playerbase : MonoBehaviour
{
    [SerializeField] private protected bool moveable;
    [Header("Balance")]
    [SerializeField] private protected int leftStick;
    [SerializeField] private protected int rightStick;
    [SerializeField] private float balanceValue=0.5f;
    [Header("Stick")]
    [SerializeField] private Stack<Transform> leftStickObjects;
    [SerializeField] private Stack<Transform> rightStickObjects;
    [SerializeField] private Transform leftStickTransform;
    [SerializeField] private Transform rightStickTransform;
    #region ConstantVariables
    private protected PlayerValues playerValues;
    private protected ConstantVariables constantVariables;
    #endregion
    #region privateVariables
    private Vector3 _mouseStartPos;
    private Vector3 _mouseEndPos;
    private int _objectIndex;
    #endregion
    private void Awake()
    {
        Setup();
        TestStart();
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

        leftStickObjects = new Stack<Transform>();
        rightStickObjects = new Stack<Transform>();
    }
    private void GetScriptableObjects()
    {
        playerValues = Resources.Load<PlayerValues>("ScriptableObjects/Player/PlayerValues");
        constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
    private void ClaimSystem()
    {
        if (LevelManager.Singleton.CurrentObject != null)
        {
            if (!LevelManager.Singleton.CurrentObject.IsUsed)
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
                LevelManager.Singleton.CurrentObject.ObjectsHolderToPlayer(objectAmount, leftStickTransform);
            }
            else
            {
                rightStick += objectAmount;
                LevelManager.Singleton.CurrentObject.ObjectsHolderToPlayer(objectAmount, rightStickTransform);
            }
            BalanceSystem(Mathf.Abs((float)objectAmount / (float)constantVariables.MaxStackableObjects), direction);
        }
    }
    private int ObjectAmount(int direction)
    {
        int givenObject=0;
        if (LevelManager.Singleton.CurrentObject.ObjectCount >0)
        {
            givenObject = LevelManager.Singleton.CurrentObject.ObjectCount;
        }
        else
        {
            if (direction == -1)
            {
                if (leftStick > 0)
                {
                    if (leftStick >= LevelManager.Singleton.CurrentObject.ObjectCount)
                    {
                        givenObject = LevelManager.Singleton.CurrentObject.ObjectCount;
                    }
                    else
                    {
                        givenObject =LevelManager.Singleton.CurrentObject.ObjectCount+ leftStick;
                    }
                }
            }
            else if (direction == 1)
            {
                if (rightStick > 0)
                {
                    if (rightStick >= LevelManager.Singleton.CurrentObject.ObjectCount)
                    {
                        givenObject = LevelManager.Singleton.CurrentObject.ObjectCount;
                    }
                    else
                    {
                        givenObject = LevelManager.Singleton.CurrentObject.ObjectCount + rightStick;
                    }
                }
            }
        }
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
        float newValue = -(value * direction);
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