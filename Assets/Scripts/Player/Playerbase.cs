using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Playerbase : MonoBehaviour
{
    #region PrivateVariables
    private PlayerValues _playerValues;
    #endregion
    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        GetScriptableObjects();
    }
    private void GetScriptableObjects()
    {
        _playerValues = Resources.Load<PlayerValues>("ScriptableObjects/Player/PlayerValues");
    }
}
