using UnityEngine;

public class BillBoardFX : MonoBehaviour
{
    public Transform camTransform;

    void FixedUpdate()
    {
        //looks at the camera
        transform.LookAt(camTransform.position);
    }
}