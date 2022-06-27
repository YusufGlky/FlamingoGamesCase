using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Constant Variables", menuName = "Constant Variables", order = 0)]
public class ConstantVariables : ScriptableObject
{
    [SerializeField] private int maxStackableObjects;
    [SerializeField] private float balanceChangeDuration;
    public int MaxStackableObjects { get => maxStackableObjects; private set => maxStackableObjects = value;}
    public float BalanceChangeDuration { get => balanceChangeDuration; private set => balanceChangeDuration = value;}
}
