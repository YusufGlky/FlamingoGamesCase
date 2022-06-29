using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoSingleton<Gamemanager>
{
    public static Action PlayAction;
    public static Action VictoryAction;
    public static Action FailedAction;
    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        Application.targetFrameRate = 60;
    }
    public void Play()
    {
        PlayAction?.Invoke();
    }
    public void Victory()
    {
        VictoryAction?.Invoke();
    }
    public void Failed()
    {
        FailedAction?.Invoke();
    }
}
