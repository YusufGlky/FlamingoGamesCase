using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBase : MonoBehaviour
{
    [SerializeField] private int objectCount;
    public int ObjectCount { get => objectCount; private set { objectCount = value; } }

    public int GiveObject(int amount)
    {
        int givenObject = 0;
        if (ObjectCount >= amount)
        {
            givenObject = ObjectCount - amount;
        }
        else
        {
            givenObject = ObjectCount;
        }
        if (ObjectCount<=0)
        {
            //To Do
            //+New Object
            //+
            //+Destroy Object
        }
        return givenObject;
    }
}
