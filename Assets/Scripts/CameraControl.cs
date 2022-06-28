using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playCam;
    private void OnEnable()
    {
        Gamemanager.PlayAction += ActivatePlayCam;
    }
    private void OnDisable()
    {
        Gamemanager.PlayAction -= ActivatePlayCam;
    }
    private void ActivatePlayCam()
    {
        playCam.Priority = 11;
    }
}
