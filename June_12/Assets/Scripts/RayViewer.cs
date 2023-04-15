using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
    public float weaponRange = 50f;
    public Transform gunEnd;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 shoot = transform.TransformDirection(Vector3.right);
        Vector3 lineOrigin = gunEnd.position;

        Debug.DrawRay(lineOrigin, shoot * weaponRange, Color.green);
	}
}
