using Cinemachine;
using UnityEngine;

public class CamTargetLock : MonoBehaviour
{
    [SerializeField] private Transform player;       
    [SerializeField] private Transform target;      
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineFreeLook defaultCamera;
    [SerializeField] private float distanceBehindPlayer = 10f;  
    [SerializeField] private float heightOffset = 1.75f;          
    [SerializeField] private float followSmoothing = 10f;       
    [SerializeField] private float targetLockDistance = 4f;   

    private bool isTargetLocked = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Mouse 3 was pressed.");
            isTargetLocked = !isTargetLocked;
            
            defaultCamera.gameObject.SetActive(!isTargetLocked);;
            SetTargetLock(isTargetLocked); 
        }
    }

    private void LateUpdate()
    {
        if (isTargetLocked && target != null)
        {
            FollowTargetLock();
        }
    }
    

    /// <summary>
    /// Moves the camera to lock onto the target while maintaining its distance from the player.
    /// </summary>
    private void FollowTargetLock()
    {
        //calculate a position relative to the midpoint between the player and target
        Vector3 directionToTarget = (target.position - player.position).normalized;
        Vector3 lockPosition = player.position - directionToTarget * targetLockDistance;

        //maintain height offset for clarity
        lockPosition.y = player.position.y + heightOffset;

        //smoothly move the camera to this position
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, lockPosition, Time.deltaTime * followSmoothing);

        //make the camera look at the target
        mainCamera.transform.LookAt(target);
    }
    
    public void SetTargetLock(bool lockOn)
    {
        isTargetLocked = lockOn;
    }
}