using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardFX : MonoBehaviour
{
    public Transform camTransform;
    void FixedUpdate()
    {
        transform.LookAt(camTransform.position);
    }
}