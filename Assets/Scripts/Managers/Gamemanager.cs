using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoSingleton<Gamemanager>
{
    public static Action PlayAction;
    public void Play()
    {
        PlayAction?.Invoke();
    }    
}
