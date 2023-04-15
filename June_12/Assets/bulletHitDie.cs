using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletHitDie : MonoBehaviour
{
    public float lifeTime = 1f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
