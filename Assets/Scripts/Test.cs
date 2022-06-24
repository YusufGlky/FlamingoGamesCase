using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private RectTransform arrowRect;
    private void Update()
    {
        ArrowRotate();
    }
    private void ArrowRotate()
    {
        float angle = value * 180;
        arrowRect.localEulerAngles = new Vector3(0, 0, -angle);
    }
}
