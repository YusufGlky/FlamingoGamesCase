using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Constant Variables", menuName = "Constant Variables", order = 0)]
public class ConstantVariables : ScriptableObject
{
    [SerializeField] private int maxStackableObjects;
    [SerializeField] private float balanceChangeDuration;
    [SerializeField] private float newObjectHolderPosZInterval;

    [Header("Object")]
    [SerializeField] private float objectMoveDuration;
    [SerializeField] private float objectMoveDelay;
    [SerializeField] private float objectMinusAnimDuration;

    [Header("UI")]
    [SerializeField] private float uiAnimDurations;
    public int MaxStackableObjects { get => maxStackableObjects; private set => maxStackableObjects = value;}
    public float BalanceChangeDuration { get => balanceChangeDuration; private set => balanceChangeDuration = value;}
    public float NewObjectHolderPosZInterval { get => newObjectHolderPosZInterval; private set => newObjectHolderPosZInterval = value;}
    public float ObjectMoveDuration { get => objectMoveDuration; private set => objectMoveDuration = value;}
    public float ObjectMoveDelay { get => objectMoveDelay; private set => objectMoveDelay = value;}
    public float ObjectMinusAnimDuration { get => objectMinusAnimDuration; private set => objectMinusAnimDuration = value;}
    public float UIAnimDurations { get => uiAnimDurations; private set => uiAnimDurations = value;}
}
