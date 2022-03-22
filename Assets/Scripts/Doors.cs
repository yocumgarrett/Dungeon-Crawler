using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform CameraTransform;
    public Camera mainCamera;
    public Vector3 cameraOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mainCamera.transform.position = CameraTransform.position + cameraOffset;
    }
}
