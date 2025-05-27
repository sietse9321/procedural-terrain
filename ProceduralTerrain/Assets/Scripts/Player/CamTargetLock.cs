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


    private void LateUpdate()
    {
        if (isTargetLocked && currentTarget != null)
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
        Vector3 directionToTarget = (currentTarget.transform.position - player.position).normalized;
        Vector3 lockPosition = player.position - directionToTarget * targetLockDistance;

        //maintain height offset for clarity
        lockPosition.y = player.position.y + heightOffset;

        //smoothly move the camera to this position
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, lockPosition, Time.deltaTime * followSmoothing);

        //make the camera look at the target
        mainCamera.transform.LookAt(currentTarget.transform);
    }
    
    public void SetTargetLock(bool lockOn)
    {
        isTargetLocked = lockOn;
    }
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
}
