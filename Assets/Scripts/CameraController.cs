using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Traget")]
    public Transform target;

    [Header("CameraOffset")]
    public float distance = 5f;
    public Vector3 camOffset;
    public float minDistance = 1f;
    public float maxDistance = 7f;

    [Header("CameraSpeed")]
    public float smoothSpeed = 5f;
    public float scrollSensitivity = 1;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (!target) 
        {
            print("Set target for the camera!");
            return;
        }

        float num = Input.GetAxis("Mouse ScrollWheel");
        distance -= num * scrollSensitivity;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Vector3 pos = target.position + camOffset;
        pos -= transform.forward * distance;

        transform.position = Vector3.Lerp(transform.position, pos, smoothSpeed * Time.deltaTime);
    }


}
