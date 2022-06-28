using DG.Tweening;
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
        Setup();
    }
    private void Setup()
    {
        SpawnNewObjectHolder();
        DOTween.SetTweensCapacity(500, 500);
    }
    public void MoveFinisher()
    {
        MoveFinisherAction?.Invoke();
    }
    public void SpawnNewObjectHolder()
    {
        int randomStackCount = 0;
        if (UnityEngine.Random.Range(0,80)<50&&(LevelManager.Singleton.LeftStickObjects>0||LevelManager.Singleton.RightStickObjects>0))
        {
            randomStackCount = UnityEngine.Random.Range(Mathf.Clamp(-100, -Mathf.Abs(LevelManager.Singleton.LeftStickObjects + LevelManager.Singleton.RightStickObjects), -1), -1);
        }
        else
        {
            randomStackCount = UnityEngine.Random.Range(1, Mathf.Max(LevelManager.Singleton.LeftStickObjects, LevelManager.Singleton.RightStickObjects) + 15);
        }
        CurrentObject = ObjectSpawnManager.Singleton.SpawnObjectAndSetPosition(randomStackCount);
    }
}
