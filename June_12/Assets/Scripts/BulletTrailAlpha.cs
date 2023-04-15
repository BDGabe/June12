using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailAlpha : MonoBehaviour
{
    LineRenderer line;
    public float alphaOne = 0.0f;
    public float alphaTwo = 0.0f;
    public float speed = 0.1f;
    public float widthStart = 0.1f;
    public float widthEnd = 0.1f;
    public Color start;
    public Color end;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        //line.material = new Material(Shader.Find("Particles/Additive (Soft)"));
    }

    void Update()
    {
        if (alphaOne >= 0 || alphaTwo >= 0)
        {
            alphaOne -= Time.deltaTime * speed;
            alphaTwo -= Time.deltaTime * speed;
        }
        start.a = alphaTwo;
        end.a = alphaOne;
        line.startColor = start;
        line.endColor = end;

        line.startWidth = widthStart;
        line.endWidth = widthEnd;
    }
}
