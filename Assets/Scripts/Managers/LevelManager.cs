using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private bool testStart;
    [SerializeField] private bool testUpdate;
    public bool TestStart { get=> testStart; private set=> testStart = value;}
    public bool TestUpdate { get=> testUpdate; private set=> testUpdate = value;}
}
