using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoSingleton<Gamemanager>
{
    public static Action PlayAction;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Play();
        }
    }
    public void Play()
    {
        PlayAction?.Invoke();
    }    
}
