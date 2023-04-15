using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzleFlashLight : MonoBehaviour
{
    RaycastShoot shootScript;

    private Light muzzle;

    void Start ()
    {
        shootScript = this.transform.parent.GetComponent<RaycastShoot>();
        muzzle = GetComponent<Light>();
        muzzle.enabled = false;
	}

    private void FixedUpdate()
    {
        if(shootScript.gunFired == true)
        {
            StartCoroutine(MuzzleEffect());
        }
    }

    public IEnumerator MuzzleEffect()
    {
        muzzle.enabled = true;
        yield return shootScript.shotDuration;
        muzzle.enabled = false;
    }
}