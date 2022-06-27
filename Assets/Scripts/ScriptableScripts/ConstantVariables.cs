using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Constant Variables", menuName = "Constant Variables", order = 0)]
public class ConstantVariables : ScriptableObject
{
    [SerializeField] private int maxStackableObjects;
    public int MaxStackableObjects { get => maxStackableObjects; private set => maxStackableObjects = value;}
}
