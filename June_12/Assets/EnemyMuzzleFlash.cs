using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMuzzleFlash : MonoBehaviour
{

    EnemyShootScript shootScript;

    private Light muzzle;

    void Start()
    {
        shootScript = this.transform.parent.GetComponent<EnemyShootScript>();
        muzzle = GetComponent<Light>();
        muzzle.enabled = false;
    }

    private void FixedUpdate()
    {
        if (shootScript.gunFired == true)
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
