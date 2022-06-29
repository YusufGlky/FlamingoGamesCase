using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playCam;
    private void OnEnable()
    {
        Gamemanager.PlayAction += ActivatePlayCam;
        Gamemanager.FailedAction += ShakeCamera;
    }
    private void OnDisable()
    {
        Gamemanager.PlayAction -= ActivatePlayCam;
        Gamemanager.FailedAction -= ShakeCamera;
    }
    private void ActivatePlayCam()
    {
        playCam.Priority = 11;
    }
    private void ShakeCamera()
    {
        playCam.transform.DOShakeRotation(0.5f,Vector3.one * 0.4f, 10, 10,true);
    }
}
