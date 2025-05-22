using UnityEngine;

public class CamTargetLock : MonoBehaviour
{
    [SerializeField] private Transform player;       // The player's transform
    [SerializeField] private Transform target;       // The target's transform for target lock
    [SerializeField] private Camera mainCamera;      // The main camera
    [SerializeField] private float distanceBehindPlayer = 5f;  // Distance behind the player
    [SerializeField] private float heightOffset = 2f;          // Height offset for the camera
    [SerializeField] private float followSmoothing = 5f;       // Smoothing factor for position updates
    [SerializeField] private float targetLockDistance = 10f;   // Distance to maintain when target lock is active

    private bool isTargetLocked = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Engage target lock with the T key
        {
            SetTargetLock(true);
        }
        else if (Input.GetKeyDown(KeyCode.F)) // Release target lock with the F key
        {
            SetTargetLock(false);
        }
    }

    private void LateUpdate()
    {
        if (isTargetLocked && target != null)
        {
            FollowTargetLock();
        }
        else
        {
            FollowPlayer();
        }
    }

    /// <summary>
    /// Moves the camera to stay behind the player.
    /// </summary>
    private void FollowPlayer()
    {
        // Calculate the position behind the player
        Vector3 behindPosition = player.position - player.forward * distanceBehindPlayer;
        behindPosition.y += heightOffset;

        // Smoothly move the camera to the position
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, behindPosition, Time.deltaTime * followSmoothing);

        // Make the camera look at the player
        mainCamera.transform.LookAt(player);
    }

    /// <summary>
    /// Moves the camera to lock onto the target while maintaining its distance from the player.
    /// </summary>
    private void FollowTargetLock()
    {
        // Calculate a position relative to the midpoint between the player and target
        Vector3 directionToTarget = (target.position - player.position).normalized;
        Vector3 lockPosition = player.position - directionToTarget * targetLockDistance;

        // Maintain height offset for clarity
        lockPosition.y = player.position.y + heightOffset;

        // Smoothly move the camera to this position
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, lockPosition, Time.deltaTime * followSmoothing);

        // Make the camera look at the target
        mainCamera.transform.LookAt(target);
    }

    /// <summary>
    /// Toggles target lock behavior on or off.
    /// </summary>
    /// <param name="lockOn">If true, lock the camera onto the target. If false, return to free-following the player.</param>
    public void SetTargetLock(bool lockOn)
    {
        isTargetLocked = lockOn;
    }
}