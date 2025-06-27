using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamTargetLock : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject targetCanvas;
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] float distanceBehindPlayer = 10f;
    [SerializeField] float heightOffset = 1.75f;
    [SerializeField] float followSmoothing = 0.1f;
    private float _switchTargetCooldown = 0.2f;
    private float _lastSwitchTime = -999f;
    private const float TargetLockDistance = 4f;
    private const float DetectionRadius = 10f;

    public ITargetable CurrentTarget { get; private set; }
    public bool IsTargetLocked { get; private set; }
    private int currentTargetIndex;
    ITargetable[] targetsInRange;
    

    public ITargetable[] DetectTargetsInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius);
        List<ITargetable> foundTargets = new List<ITargetable>();

        foreach (Collider col in colliders)
        {
            ITargetable target = col.GetComponentInParent<ITargetable>();
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

        if (targetsInRange.Length == 0)
            return;

        CurrentTarget = targetsInRange[currentTargetIndex];
        IsTargetLocked = !IsTargetLocked;

        defaultCamera.gameObject.SetActive(!IsTargetLocked);
        targetCanvas.SetActive(IsTargetLocked);
    }
    public void SwitchTarget(int direction)
    {
        if (Time.time - _lastSwitchTime > _switchTargetCooldown)
        {
            _lastSwitchTime = Time.time;
            currentTargetIndex = (currentTargetIndex + direction) % targetsInRange.Length;

            if (currentTargetIndex < 0)
            {
                currentTargetIndex += targetsInRange.Length;
            }

            CurrentTarget = targetsInRange[currentTargetIndex];
        }
    }
    
    public void CheckTargetLock()
    {
        Debug.Log("current target= " + CurrentTarget);
        if (CurrentTarget == null || Array.IndexOf(targetsInRange, CurrentTarget) == -1)
        {
            Debug.Log("target is null or not in range");
            if (targetsInRange.Length > 0)
            {
                currentTargetIndex = 0; 
                CurrentTarget = targetsInRange[currentTargetIndex];
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
        targetsInRange = DetectTargetsInRadius();
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