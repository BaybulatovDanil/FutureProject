using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 camOffset = new Vector3(1.8f, 8.7f, -2.96f);
    private Transform target;

    private void Start()
    {
        target = GameObject.Find("Character").transform;
    }

    private void LateUpdate()
    {
        this.transform.position = target.TransformPoint(camOffset);
        this.transform.LookAt(target);
    }

}
