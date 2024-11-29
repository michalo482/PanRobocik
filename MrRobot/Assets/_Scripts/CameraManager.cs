using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineFramingTransposer _transposer;

    [SerializeField] private float _targetCameraDistance;
    [SerializeField] private float _distanceChangeRate;

    [SerializeField] private bool canChangeCameraDistance;
    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        UpdateCameraDistance();
    }

    private void UpdateCameraDistance()
    {
        
        if(canChangeCameraDistance == false)
            return;
        float currentDistance = _transposer.m_CameraDistance;

        if (Mathf.Abs(_targetCameraDistance - currentDistance) > .01f)
        {
            _transposer.m_CameraDistance = Mathf.Lerp(currentDistance, _targetCameraDistance,
                _distanceChangeRate * Time.deltaTime);
        }
    }

    public void ChangeCameraDistance(float distance) => _targetCameraDistance = distance;
}
