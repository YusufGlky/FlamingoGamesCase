using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [SerializeField] private float leftStick;
    [SerializeField] private float rightStick;
    [SerializeField] private float balance;
    private void Update()
    {
        
        if (leftStick==rightStick)
        {
            balance = Mathf.MoveTowards(balance, 0.5f, Mathf.Abs(leftStick - rightStick) * Time.deltaTime);
        }
        else if (leftStick > rightStick)
        {
            balance = Mathf.MoveTowards(balance, 0, Mathf.Abs(leftStick - rightStick) * Time.deltaTime);
        }
        else if (leftStick < rightStick)
        {
            balance = Mathf.MoveTowards(balance, 1, Mathf.Abs(leftStick - rightStick) * Time.deltaTime);
        }
    }
}
