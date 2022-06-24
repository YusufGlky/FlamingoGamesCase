using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Player Values",menuName ="Player/PlayerValues")]
public class PlayerValues : ScriptableObject
{
    [SerializeField] private float playerSpeed;
    public float PlayerSpeed { get=>playerSpeed; private set=>playerSpeed=value; }
}
