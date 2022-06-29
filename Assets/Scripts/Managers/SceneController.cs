using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoSingleton<SceneController>
{
    [SerializeField] private GameObject sceneLoaderParent;
    [SerializeField] private Image sceneLoader;
    [SerializeField] private float sceneLoadDuration;
    private void Awake()
    {
        Setup();
    }
    public void LoadLevel(int levelIndex)
    {
        SceneLoaderParentState(true);
        sceneLoader.DOFade(1, sceneLoadDuration).OnComplete(() => LevelLoader(levelIndex));
    }
    private void Setup()
    {
        sceneLoader.DOFade(0, sceneLoadDuration).OnComplete(() => SceneLoaderParentState(false));
    }   
    private void LevelLoader(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    private void SceneLoaderParentState(bool activate)
    {
        sceneLoaderParent.SetActive(activate);
    }
}
