using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Playerbase : MonoBehaviour
{
    [SerializeField] private protected bool moveable;
    #region ConstantVariables
    private protected PlayerValues playerValues;
    private protected ConstantVarýables constantVariables;
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
    private void GetScriptableObjects()
    {
        playerValues = Resources.Load<PlayerValues>("ScriptableObjects/Player/PlayerValues");
        constantVariables = Resources.Load<ConstantVarýables>("ScriptableObjects/ConstantVariables");
    }
    private void Movement()
    {
        if (moveable)
        {
            transform.Translate(Vector3.forward * playerValues.PlayerSpeed*Time.deltaTime);
        }
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

        }
    }
}