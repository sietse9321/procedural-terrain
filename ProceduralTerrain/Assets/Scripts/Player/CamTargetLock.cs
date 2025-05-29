using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CamTargetLock : MonoBehaviour
{
    [SerializeField] Transform player;       
    [SerializeField] Enemy currentTarget;
    [SerializeField] Enemy[] enemiesInRange;
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] float distanceBehindPlayer = 10f;  
    [SerializeField] float heightOffset = 1.75f;          
    [SerializeField] float followSmoothing = 10f;       
    [SerializeField] float targetLockDistance = 4f;   
    
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
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, lockPosition, Time.deltaTime * followSmoothing);

        //make the camera look at the target
        mainCamera.transform.LookAt(currentTarget.transform);
    }

    private void DetectTargetsInRange()
    {
        //detect enemies in range
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
            isTargetLocked = !isTargetLocked;

            defaultCamera.gameObject.SetActive(!isTargetLocked);
            SetTargetLock(isTargetLocked); 
            
            //change to detect targets in range
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
