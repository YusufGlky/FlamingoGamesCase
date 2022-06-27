using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Overlay : MonoSingleton<Overlay>
{
    [Header("BalanceMeteer")]
    [SerializeField] private RectTransform meteerParent;
    [SerializeField] private RectTransform arrowRect;
    [SerializeField] private float arrowAnimDuration;
    [SerializeField] private float balanceValue;
    public void BalanceSystem(float value, int direction)
    {
        float newValue = -(value * direction);
        if (DOTween.IsTweening(balanceValue))
        {
            DOTween.Kill(balanceValue);
        }
        DOVirtual.Float(balanceValue, balanceValue + newValue, arrowAnimDuration, x =>
        {
            balanceValue = x;
            arrowRect.localEulerAngles = new Vector3(0, 0, -(balanceValue* 180));
            if (balanceValue <= -1 || balanceValue >= 1)
            {
                //To Do: Death
            }
        });
    }   
}
