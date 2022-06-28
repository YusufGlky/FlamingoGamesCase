using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoSingleton<ObjectManager>
{
    [Header("Object")]
    [SerializeField] private ObjectBase currentObject;
    public ObjectBase CurrentObject { get => currentObject; private set => currentObject = value; }

    public static Action MoveFinisherAction;
    private void Start()
    {
        SpawnNewObjectHolder(new Vector2Int(12, 15));
    }
    public void MoveFinisher()
    {
        MoveFinisherAction?.Invoke();
    }
    public void SpawnNewObjectHolder(Vector2Int minMaxExclusive)
    {
        CurrentObject = ObjectSpawnManager.Singleton.SpawnObjectAndSetPosition(UnityEngine.Random.Range(minMaxExclusive.x, minMaxExclusive.y));
    }
}
