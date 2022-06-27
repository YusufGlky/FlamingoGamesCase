using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Playerbase : MonoBehaviour,IClaimable
{
    [SerializeField] private protected bool moveable;
    [Header("Balance")]
    [SerializeField] private protected int leftStick;
    [SerializeField] private protected int rightStick;

    #region ConstantVariables
    private protected PlayerValues playerValues;
    private protected ConstantVariables constantVariables;
    #endregion
    #region privateVariables
    private Vector3 _mouseStartPos;
    private Vector3 _mouseEndPos;
    private int _direction;
    #endregion
    private void Awake()
    {
        Setup();
        TestStart();
    }
    private void Setup()
    {
        GetScriptableObjects();
    }
    private void Update()
    {
        Movement();
        TestUpdate();
    }
    public void ClaimObject(int objectAmount)
    {
        switch (ClaimDirection())
        {
            case -1://Left
                leftStick += objectAmount;
                Overlay.Singleton.BalanceSystem(Mathf.Abs((float)objectAmount / (float)constantVariables.MaxStackableObjects),-1);
                break;
            case 0://Death
                break;
            case 1://Right
                rightStick += objectAmount;
                Overlay.Singleton.BalanceSystem(Mathf.Abs((float)objectAmount / (float)constantVariables.MaxStackableObjects), 1);
                break;
        }
    }
    private void GetScriptableObjects()
    {
        playerValues = Resources.Load<PlayerValues>("ScriptableObjects/Player/PlayerValues");
        constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
    private void Movement()
    {
        if (moveable)
        {
            transform.Translate(Vector3.forward * playerValues.PlayerSpeed*Time.deltaTime);
        }
    }
    private int ClaimDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartPos = Input.mousePosition;
            _direction = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseEndPos = Input.mousePosition;
            if (Mathf.Abs(_mouseStartPos.x - _mouseEndPos.x) > 120)
            {
                if (_mouseStartPos.x > _mouseEndPos.x)//Left
                {
                    _direction = -1;
                }
                if (_mouseStartPos.x < _mouseEndPos.x)//Right
                {
                    _direction = 1;
                }
            }
        }
        return _direction;
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
                _direction = -1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _direction = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))//LeftStick
        {
            ClaimObject(10);
        }
        if (Input.GetKeyDown(KeyCode.D))//RightStick
        {
            ClaimObject(-30);
        }
    }

}