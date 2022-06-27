using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Overlay : MonoSingleton<Overlay>
{
    [Header("BalanceMeteer")]
    [SerializeField] private RectTransform meteerParent;
    [SerializeField] private RectTransform arrowRect;
    public void BalanceMeteer(float value)
    {
        arrowRect.localEulerAngles = new Vector3(0, 0, -(value * 180));
    }  
}
