using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0f;

    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        Vector3 myPos = target.position + offset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, myPos, ref velocity, smoothSpeed * Time.deltaTime);

        transform.position = smoothPos;

        transform.LookAt(target);
    }
}
