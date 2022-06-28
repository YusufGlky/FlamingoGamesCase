using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [SerializeField] private int groundPizza;
    [SerializeField] private int myPizza;
    [SerializeField] private int result;
    private void Update()
    {
        result = (groundPizza + myPizza)-myPizza;
    }
    private void ArrowRotate()
    {
    }
}
