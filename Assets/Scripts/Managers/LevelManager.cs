using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerDistance;

    [Header("Object")]
    [SerializeField] private ObjectBase currentObject;
    #region Properties
    public ObjectBase CurrentObject { get => currentObject; private set => currentObject = value; }
    public Transform PlayerTransform { get => playerTransform; private set => playerTransform = value; }
    public float PlayerDistance { get => playerDistance; private set => playerDistance = value; }
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
    private void Update()
    {
        ObjectToPlayerDistance();
    }
    private void Setup()
    {
        PlayerTransform = FindObjectOfType<Playerbase>().transform;
    }
    private void ObjectToPlayerDistance()
    {
        if (PlayerTransform!=null&&CurrentObject!=null)
        {
            playerDistance = Mathf.Abs(PlayerTransform.position.z - CurrentObject.transform.position.z);
        }
    }
}
