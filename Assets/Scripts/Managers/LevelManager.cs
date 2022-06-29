using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int leftStickObjects;
    [SerializeField] private int rightStickObjects;

    [Header("Finish")]
    [SerializeField] private Transform finishLine;
    #region Properties
    public Transform PlayerTransform { get => playerTransform; private set => playerTransform = value; }
    public Transform FinishLine { get => finishLine; private set => finishLine = value; }
    public int LeftStickObjects { get => leftStickObjects; private set => leftStickObjects = value; }
    public int RightStickObjects { get => rightStickObjects; private set => rightStickObjects = value; }
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
    public void UpdateStickObjectCount(int leftStick, int rightStick)
    {
        LeftStickObjects = leftStick;
        RightStickObjects = rightStick;
    }
    private void Setup()
    {
        PlayerTransform = FindObjectOfType<Playerbase>().transform;
    }
}
