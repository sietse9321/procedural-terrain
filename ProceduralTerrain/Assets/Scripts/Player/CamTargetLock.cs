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
    [SerializeField] float targetLockDistance = 4f;
    private float detectionRadius = 5f;


    private bool isTargetLocked = false;
    private int currentTargetIndex = 0;

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
        mainCamera.transform.LookAt(currentTarget.transform);
    }


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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


    private void SwitchTarget(int direction)
    {
        currentTargetIndex = (currentTargetIndex + direction) % enemiesInRange.Length;

        if (currentTargetIndex < 0)
        {
            currentTargetIndex += enemiesInRange.Length;
        }

        currentTarget = enemiesInRange[currentTargetIndex];
    }

    public void SetTargetLock(bool lockOn)
    {
        isTargetLocked = lockOn;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            //make constant detection (fixed update)
            //check if there are no targets in range (error exception)
            enemiesInRange = DetectEnemiesInRadius();
            currentTarget = enemiesInRange[currentTargetIndex];
            isTargetLocked = !isTargetLocked;

            defaultCamera.gameObject.SetActive(!isTargetLocked);
            SetTargetLock(isTargetLocked);
        }

        if (isTargetLocked && Input.mouseScrollDelta.y != 0f)
        {
            SwitchTarget((int)Mathf.Sign(Input.mouseScrollDelta.y));
        }
    }

    private void LateUpdate()
    {
        if (isTargetLocked && currentTarget)
        {
            FollowTargetLock();
        }
    }
}