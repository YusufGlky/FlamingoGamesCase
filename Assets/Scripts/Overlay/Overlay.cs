using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Overlay : MonoSingleton<Overlay>
{
    [Header("BalanceMeteer")]
    [SerializeField] private RectTransform meteerParent;
    [SerializeField] private RectTransform arrowRect;

    [Header("Menu")]
    [SerializeField] private GameObject mainMenu;

    [Header("RetryButton")]
    [SerializeField] private RectTransform retryButton;
    [Tooltip("Anchor Pos")][SerializeField] private float retryButtonYPos;
    #region ConstantVariables
    private ConstantVariables _constantVariables;
    #endregion
    private void Awake()
    {
        Setup();
    }
    private void OnEnable()
    {
        SetActions(true);
    }
    private void OnDisable()
    {
        SetActions(false);
    }
    public void TapToStart()
    {
        Gamemanager.Singleton.Play();
        meteerParent.DOScale(1, 0.3f);
        mainMenu.SetActive(false);
    }
    public void BalanceMeteer(float value)
    {
        arrowRect.localEulerAngles = new Vector3(0, 0, -(value * 180));
    }  
    public void RetryButton()
    {
        SceneController.Singleton.LoadLevel(0);
    }
    private void SetActions(bool enabled)
    {
        if (enabled)
        {
            Gamemanager.FailedAction += Failed;
        }
        else
        {
            Gamemanager.FailedAction -= Failed;
        }
    }
    private void Setup()
    {
        GetScriptableObjects();
    }
    private void GetScriptableObjects()
    {
        _constantVariables = Resources.Load<ConstantVariables>("ScriptableObjects/ConstantVariables");
    }
    private void Failed()
    {
        RetryButtonAnimator();
    }
    private void RetryButtonAnimator()
    {
        retryButton.DOAnchorPosY(retryButtonYPos, _constantVariables.UIAnimDurations);
    }
}
