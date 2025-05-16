using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    private void Update()
    {
        gameObject.transform.position = cameraHolder.position;
    }
}
