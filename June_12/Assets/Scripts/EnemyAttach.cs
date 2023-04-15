using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttach : MonoBehaviour
{
    public Transform target;

    public Vector3 offset1;
    public Vector3 offset2;

    private EnemyShootScript shootScript;
    private EnemyShootScriptLeft shootScriptLeft;
    private SpriteRenderer sprites;

    public static float angle;

    public float speed = 5f;
    public float maxDistance;

    public bool facingRight;

    public Transform player;
    public LineOfSight enemySight;

    float nextClick;

    public bool playerWasCaught = false;

    // Use this for initialization
    void Start ()
    {
        if(GetComponent<EnemyShootScript>() != null)
        {
            shootScript = GetComponent<EnemyShootScript>();
        }
        else if (GetComponent<EnemyShootScriptLeft>() != null)
        {
            shootScriptLeft = GetComponent<EnemyShootScriptLeft>();
        }
        sprites = GetComponent<SpriteRenderer>();

        //print(sprites);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //print(sprites + " are in fixedupdate");
        /*
        var claudiaLinBanks = player.transform.position;

        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(claudiaLinBanks.x - screenPoint.x, claudiaLinBanks.y - screenPoint.y);
        angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
        */
        if ((enemySight.playerSighted == true) && (Vector2.Distance(transform.position, player.position) < maxDistance))
        {
            playerWasCaught = true;
            //print(sprites + " and " + enemySight.playerSighted);
        }

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion qAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.localRotation = Quaternion.Slerp(transform.rotation, qAngle, speed * Time.deltaTime);

        Vector3 myPos;

        //print(sprites + " and did math");

        if (angle > 0f && angle < 90f || angle < 0f && angle > -90f)
        {
            myPos = target.position + offset2;
            transform.position = myPos;

            //print(sprites + " angles");
        }
        else if (angle > 90f && angle < 180f || angle < -90f && angle > -180f)
        {
            myPos = target.position + offset1;
            transform.position = myPos;

            //print(sprites + " and angles");
        }


        if((playerWasCaught) && (Vector2.Distance(transform.position, player.position) < maxDistance + 0.1f))
        {
            compOn(playerWasCaught);
        }
        else
        {
            compOn(false);
        }
    }

    void compOn(bool on)
    {
        if (GetComponent<EnemyShootScript>() != null)
        {
            shootScript = GetComponent<EnemyShootScript>();
            shootScript.enabled = on;
        }
        else if (GetComponent<EnemyShootScriptLeft>() != null)
        {
            shootScriptLeft = GetComponent<EnemyShootScriptLeft>();
            shootScriptLeft.enabled = on;
        }
        sprites.enabled = on;
        //print(sprites + " are " + sprites.enabled);
    }
}
