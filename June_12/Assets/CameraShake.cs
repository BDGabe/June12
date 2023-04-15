using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCam;

    public float shakeNum = 0f;
    public float shakeLength = 0f;

    void Awake()
    {
        if(mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            shake(shakeNum, shakeLength);
        }
    }

    public void shake(float amount, float length)
    {
        shakeNum = amount;

        InvokeRepeating("doShake", 0, 0.01f);

        Invoke("stopShake", length);
    }

    void doShake()
    {
        if(shakeNum > 0)
        {
            Vector3 camPosition = mainCam.transform.position;

            float offsetX = Random.value * shakeNum * 2 - shakeNum;
            float offsetY = Random.value * shakeNum * 2 - shakeNum;
            float offsetZ = Random.value * shakeNum * 2 - shakeNum;

            camPosition.x += offsetX;
            camPosition.y += offsetY;
            camPosition.z += offsetZ;

            mainCam.transform.position = camPosition;
        }
    }

    void stopShake()
    {
        CancelInvoke("doShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
