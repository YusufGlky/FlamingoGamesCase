using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    #region Properties
    public Transform PlayerTransform { get => playerTransform; private set => playerTransform = value; }
    #endregion
    #region TestVariables
    [Header("Test")]
    [SerializeField] private bool testStart;
    [SerializeField] private bool testUpdate;
    public bool TestStart { get => testStart; private set => testStart = value; }
    public bool TestUpdate { get => testUpdate; private set => testUpdate = value; }
    #endregion
    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        PlayerTransform = FindObjectOfType<Playerbase>().transform;
    }
}
