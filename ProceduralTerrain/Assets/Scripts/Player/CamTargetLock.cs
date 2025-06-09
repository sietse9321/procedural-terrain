using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CamTargetLock : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject targetCanvas;
    [SerializeField] Enemy currentTarget;
    [SerializeField] Enemy[] enemiesInRange;
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] float distanceBehindPlayer = 10f;
    [SerializeField] float heightOffset = 1.75f;
    [SerializeField] float followSmoothing = 10f;
    private const float targetLockDistance = 4f;
    private const float detectionRadius = 10f;


    private bool isTargetLocked = false;
    private int currentTargetIndex = 0;
    public Enemy[] DetectEnemiesInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        List<Enemy> foundEnemies = new List<Enemy>();

        foreach (Collider col in colliders)
        {
            Enemy enemy = col.GetComponentInParent<Enemy>();
            if (enemy && !foundEnemies.Contains(enemy))
            {
                foundEnemies.Add(enemy);
            }
        }

        return foundEnemies.ToArray();
    }

    /// <summary>
    /// Moves the camera to lock onto the target while maintaining its distance from the player.
    /// </summary>
    private void FollowTargetLock()
    {
        //calculate a position relative to the midpoint between the player and target
        Vector3 directionToTarget = (currentTarget.transform.position - player.position).normalized;
        Vector3 lockPosition = player.position - directionToTarget * targetLockDistance;

        //maintain height offset for clarity
        lockPosition.y = player.position.y + heightOffset;

        //smoothly move the camera to this position
        mainCamera.transform.position =
            Vector3.Lerp(mainCamera.transform.position, lockPosition, Time.deltaTime * followSmoothing);

        //make the camera look at the target
        targetCanvas.transform.position = currentTarget.transform.position;
        mainCamera.transform.LookAt(currentTarget.transform);
    }

    /// <summary>
    /// Debug Sphere to see how big the range is
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    private void SetTargetLock(bool lockOn)
    {
        isTargetLocked = lockOn;
    }

    
    public void TargetLock()
    {
        currentTarget = null;
        SetTargetLock(isTargetLocked);

        if (enemiesInRange.Length == 0)
            return;

        currentTarget = enemiesInRange[currentTargetIndex];
        isTargetLocked = !isTargetLocked;

        defaultCamera.gameObject.SetActive(!isTargetLocked);
        targetCanvas.SetActive(isTargetLocked);
    }
    public void SwitchTarget(int direction)
    {
        currentTargetIndex = (currentTargetIndex + direction) % enemiesInRange.Length;

        if (currentTargetIndex < 0)
        {
            currentTargetIndex += enemiesInRange.Length;
        }

        currentTarget = enemiesInRange[currentTargetIndex];
    }
    
    public void CheckTargetLock()
    {
        if (currentTarget == null || Array.IndexOf(enemiesInRange, currentTarget) == -1)
        {
            if (enemiesInRange.Length > 0)
            {
                currentTargetIndex = 0; 
                currentTarget = enemiesInRange[currentTargetIndex];
            }
            else
            {
                isTargetLocked = false;
                defaultCamera.gameObject.SetActive(true);
                targetCanvas.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        enemiesInRange = DetectEnemiesInRadius();
    }
    
    void Update()
    {
        if (!isTargetLocked) return;
        CheckTargetLock();
    }

    private void LateUpdate()
    {
        if (isTargetLocked && currentTarget)
        {
            FollowTargetLock();
        }
    }
}