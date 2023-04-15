using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmFollow : MonoBehaviour
{
    public static float angle;
    public float aimOff = 3;
    bool facingRight = true;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float move = mcControllerScript.move;

        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);

        Quaternion qAngle = Quaternion.Euler(0, 0, angle - aimOff);

        transform.localRotation = qAngle;

        if (angle > 0f && angle < 90f || angle < 0f && angle > -90f)
        {
            if (facingRight == false)
            {
                FlipAnim();
                aimOff = aimOff * -1;
            }
        }
        else if (angle > 90f && angle < 180f || angle < -90f && angle > -180f)
        {
            if (facingRight == true)
            {
                FlipAnim();
                aimOff = aimOff * -1;
            }
        }
    }

    void FlipAnim()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;
    }
}
