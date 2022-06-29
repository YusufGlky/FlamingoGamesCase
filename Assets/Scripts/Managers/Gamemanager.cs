using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoSingleton<Gamemanager>
{
    public static Action PlayAction;
    public static Action VictoryAction;
    public static Action FailedAction;
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
