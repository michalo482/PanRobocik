using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAim : MonoBehaviour
{
    private Player _player;
    private PlayerControlls _controls;

    [Header("Aim visuals")] 
    [SerializeField] private LineRenderer aimLaser;

    [Header("Aim control")] 
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget;

    [Header("Camera control")] 
    [Range(0.5f, 1f)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(2f, 3.5f)]
    [SerializeField] private float maxCameraDistance = 4f;
    [Range(3f, 7f)]
    [SerializeField] private float cameraSensivity = 5f;
    [SerializeField] private LayerMask aimLayerMask;
    [FormerlySerializedAs("aim")] [SerializeField] private Transform cameraTarget;
    private Vector2 _mouseInput;
    private RaycastHit _lastKnownMouseHit;


    private void Start()
    {
        _player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        if(_player.PlayerHealth.IsDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isAimingPrecisly = !isAimingPrecisly;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            isLockingToTarget = !isLockingToTarget;
        }

        UpdateAimVisuals();
        
        UpdateAimPosition();

        UpdateCameraPosition();
    }

    private void UpdateAimVisuals()
    {
        aimLaser.enabled = _player.Weapon.WeaponReady();
        
        if(!aimLaser.enabled)
            return;

        WeaponModel weaponModel = _player.WeaponVisuals.CurrentWeaponModel();
        
        weaponModel.transform.LookAt(aim);
        weaponModel.GunPoint.LookAt(aim);
        
        float laserTipLength = 0.5f;
        Transform gunPoint = _player.Weapon.GunPoint();
        Vector3 laserDirection = _player.Weapon.BulletDirection();
        float gunDistance = _player.Weapon.CurrentWeapon().GunDistance;
        
        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLength = 0f;
        }
        
        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }

    public Transform Target()
    {
        Transform target = null;

        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        return target;
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensivity * Time.deltaTime);
    }

    private void UpdateAimPosition()
    {
        Transform target = Target();
        if (target != null && isLockingToTarget)
        {
            if (target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
            {
                aim.position = target.position;
            }
            
            return;
        }
        aim.position = GetMouseHitInfo().point;
        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, aim.position.y + 1, aim.position.z);
    }

    private Vector3 DesiredCameraPosition()
    {
        float actualMaxCameraDistance = _player.Movement.moveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;
        
        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;
        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mouseInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            _lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return _lastKnownMouseHit;
    }

    public bool CanAimPrecisly()
    {
        if (isAimingPrecisly)
        {
            return true;
        }

        return false;
    }

    private void AssignInputEvents()
    {
        _controls = _player.Controls;
        
        _controls.Character.Aim.performed += context => _mouseInput = context.ReadValue<Vector2>();
        _controls.Character.Aim.canceled += context => _mouseInput = Vector2.zero;
    }

    public Transform Aim()
    {
        return aim;
    }
}
