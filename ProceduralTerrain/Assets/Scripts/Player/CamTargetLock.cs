using System;
using System.Collections.Generic;
using Cinemachine;
using Interfaces;
using UnityEngine;

public class CamTargetLock : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject targetCanvas;
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] float heightOffset = 1.75f;
    [SerializeField] float followSmoothing = 0.1f;
    private float _switchTargetCooldown = 0.2f;
    private float _lastSwitchTime = -999f;
    private const float TargetLockDistance = 5f;
    private const float DetectionRadius = 10f;

    public IEnemy CurrentTarget { get; private set; }
    public bool IsTargetLocked { get; private set; }
    private int _currentTargetIndex;
    IEnemy[] _targetsInRange;

    private IEnemy[] DetectTargetsInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius);
        List<IEnemy> foundTargets = new List<IEnemy>();

        foreach (Collider col in colliders)
        {
            IEnemy target = col.GetComponentInParent<IEnemy>();
            if (target != null && !foundTargets.Contains(target))
            {
                foundTargets.Add(target);
            }
        }

        return foundTargets.ToArray();
    }

    private void FollowTargetLock()
    {
        Vector3 directionToTarget = (CurrentTarget.TargetTransform.position - player.position).normalized;
        Vector3 lockPosition = player.position - directionToTarget * TargetLockDistance;
        lockPosition.y = player.position.y + heightOffset;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, lockPosition, Time.deltaTime * followSmoothing *2);

        targetCanvas.transform.position = CurrentTarget.TargetTransform.position;

        Quaternion targetRotation = Quaternion.LookRotation(CurrentTarget.TargetTransform.position - mainCamera.transform.position);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * followSmoothing);
    }


    /// <summary>
    /// Debug Sphere to see how big the range is
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
    private void SetTargetLock(bool lockOn)
    {
        IsTargetLocked = lockOn;
    }

    
    public void TargetLock()
    {
        CurrentTarget = null;
        SetTargetLock(IsTargetLocked);

        if (_targetsInRange.Length == 0)
            return;

        CurrentTarget = _targetsInRange[_currentTargetIndex];
        IsTargetLocked = !IsTargetLocked;

        defaultCamera.gameObject.SetActive(!IsTargetLocked);
        targetCanvas.SetActive(IsTargetLocked);
    }
    public void SwitchTarget(int direction)
    {
        if (Time.time - _lastSwitchTime > _switchTargetCooldown)
        {
            _lastSwitchTime = Time.time;
            _currentTargetIndex = (_currentTargetIndex + direction) % _targetsInRange.Length;

            if (_currentTargetIndex < 0)
            {
                _currentTargetIndex += _targetsInRange.Length;
            }

            CurrentTarget = _targetsInRange[_currentTargetIndex];
        }
    }
    
    public void CheckTargetLock()
    {
        Debug.Log("current target= " + CurrentTarget);
        if (CurrentTarget == null || Array.IndexOf(_targetsInRange, CurrentTarget) == -1)
        {
            Debug.Log("target is null or not in range");
            if (_targetsInRange.Length > 0)
            {
                _currentTargetIndex = 0; 
                CurrentTarget = _targetsInRange[_currentTargetIndex];
            }
            else
            {
                IsTargetLocked = false;
                defaultCamera.gameObject.SetActive(true);
                targetCanvas.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        _targetsInRange = DetectTargetsInRadius();
    }
    
    void Update()
    {
        if (!IsTargetLocked) return;
        CheckTargetLock();
    }

    private void LateUpdate()
    {
        if (IsTargetLocked && CurrentTarget != null)
        {
            FollowTargetLock();
        }
    }
}