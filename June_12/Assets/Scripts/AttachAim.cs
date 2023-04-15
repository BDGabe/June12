using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachAim : MonoBehaviour
{
    public Transform target;

    public Vector3 offset1;
    public Vector3 offset2;

    private RaycastShoot shootScript;
    private SpriteRenderer sprites;

    public static float angle;
    
    public float count = 1;
    public float buttonClickRate = 0.25f;

    public bool facingRight;

    float nextClick;

    private void Start()
    {
        shootScript = GetComponent<RaycastShoot>();
        sprites = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);

        Vector3 myPos;

        if (angle > 0f && angle < 90f || angle < 0f && angle > -90f)
        {
            myPos = target.position + offset2;
            transform.position = myPos;
         
        }
        else if (angle > 90f && angle < 180f || angle < -90f && angle > -180f)
        {
            myPos = target.position + offset1;
            transform.position = myPos;
        
        }

        if (Input.GetKey(KeyCode.Mouse1) && Time.time > nextClick)
        {
            nextClick = Time.time + buttonClickRate;

            count = count * -1;

            if(count == 1)
            {
                compOn(true);
            }
            else
            {
                compOn(false);
            }

        }
    }

    void compOn(bool on)
    {
        shootScript.enabled = on;
        sprites.enabled = on;
    }
}
