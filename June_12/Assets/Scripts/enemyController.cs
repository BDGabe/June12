using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float speed = 0f;
    public float sprint = 0f;
    public float maxDistance;
    public float stoppingDistance;
    public float retreatDistance;
    public float jumpForce = 50f;
    public float mag = 0f;
    public float distToGround = 0;
    public float angle;
    public float buttonClickRate = 0.25f;
    public float groundDetectDist = 2f;

    /*
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    */

    public static float move;

    public Vector3 normal = Vector3.up;
    public Transform groundCheck;
    public Transform groundRight;
    public Transform groundLeft;
    public LayerMask whatIsGround;
    public GameObject armOne;
    public GameObject armTwo;

    bool grounded = false;
    bool groundR = false;
    bool groundL = false;
    bool facingRight = true;
    bool gunDrawn = false;
    bool walkForwards = true;
    bool walkToPlayer = true;
    bool walkAwayPlayer = true;
    bool playerSeen = false;

    float groundRadius = 0.15f;
    public float gunDrawnCount = 1;
    float nextClick;


    Rigidbody enemy;
    Animator anim;
    public LineOfSight enemySight;
    EnemyShootScript shootRight;
    EnemyShootScriptLeft shootLeft;
    public SpriteRenderer spriteRenderRight;
    public SpriteRenderer spriteRenderLeft;

    public Transform player;

	// Use this for initialization
	void Start ()
    {
        shootRight = armOne.GetComponent<EnemyShootScript>();
        shootLeft = armTwo.GetComponent<EnemyShootScriptLeft>();
        spriteRenderRight = armOne.GetComponent<SpriteRenderer>();
        spriteRenderLeft = armTwo.GetComponent<SpriteRenderer>();

        enemy = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        enemySight = this.transform.GetComponentInChildren<LineOfSight>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(enemySight.playerSighted == true)
        {
            playerSeen = true;
        }

        move = enemy.velocity.x;

        grounded = Physics.CheckSphere(groundCheck.position, groundRadius, whatIsGround);
        groundR = Physics.CheckSphere(groundRight.position, groundRadius, whatIsGround);
        groundL = Physics.CheckSphere(groundLeft.position, groundRadius, whatIsGround);

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion qAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        grounded = Physics.CheckSphere(groundCheck.position, groundRadius, whatIsGround);

        mag = enemy.velocity.magnitude;

        anim.SetBool("GunDrawn", gunDrawn);
        anim.SetBool("Ground", grounded);
        anim.SetBool("WalkToPlayer", walkToPlayer);
        anim.SetBool("WalkAwayPlayer", walkAwayPlayer);
        anim.SetBool("WalkingForwards", walkForwards);
        anim.SetFloat("vSpeed", enemy.velocity.y);
        anim.SetFloat("Speed", enemy.velocity.magnitude);
        anim.SetFloat("Angle", angle);

        if ((groundR == false) || (groundL == false))
        {
            if ((groundR == false) && (groundL == true) && ((Vector2.Distance(transform.position, player.position) > retreatDistance)) && (grounded != false))
            {
                enemy.velocity = new Vector2(speed * 1, enemy.velocity.x);
            }
            else if ((groundR == true) && (groundL == false) && ((Vector2.Distance(transform.position, player.position) > retreatDistance)) && (grounded != false))
            {
                enemy.velocity = new Vector2(speed * -1, enemy.velocity.x);
            }
            else
            {
                if(Vector2.Distance(transform.position, player.position) <= retreatDistance)
                {
                    transform.position = this.transform.position;
                }
                walkToPlayer = false;
                walkAwayPlayer = false;
                gunDrawn = true;
            }
        }

        else if (playerSeen == true)
        {
            if ((Vector2.Distance(transform.position, player.position) >= maxDistance) && Time.time > nextClick)
            {
                nextClick = Time.time + buttonClickRate;

                 gunDrawnCount = gunDrawnCount * -1;

                 if (gunDrawnCount == 1)
                 {
                    gunDrawn = true;
                }
                 else
                 {
                    gunDrawn = false;
                }
            }

            if ((Vector2.Distance(transform.position, player.position) >= maxDistance) && (grounded != false))
            {
                gunDrawn = false;
            }
            else if ((Vector2.Distance(transform.position, player.position) >= stoppingDistance) && (grounded != false))
            {
                enemy.velocity = (player.transform.position - transform.position).normalized * speed;
                walkToPlayer = true;
                walkAwayPlayer = false;
                gunDrawn = true;
            }

            else if ((Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) && (grounded != false))
            {
                transform.position = this.transform.position;
                walkToPlayer = false;
                walkAwayPlayer = false;
                gunDrawn = true;
            }

            else if (((Vector2.Distance(transform.position, player.position) <= retreatDistance)) && (grounded != false))
            {
                enemy.velocity = (player.transform.position - transform.position).normalized * -speed;
                walkToPlayer = false;
                walkAwayPlayer = true;
                gunDrawn = true;
            }
        }
        else
        {
            transform.position = this.transform.position;
            walkToPlayer = false;
            walkAwayPlayer = false;
            gunDrawn = false;
        }

        if (gunDrawn == true)
        {
            shootRight.enabled = true;
            shootLeft.enabled = true;
            spriteRenderRight.enabled = true;
            spriteRenderLeft.enabled = true;
            //print(spriteRenderRight.enabled + " and " + spriteRenderLeft.enabled);
            if (angle >= 0f && angle <= 90f || angle < 0f && angle >= -90f)
            {
                if (facingRight == false)
                {
                    FlipAnim();
                }

                if (move > 0)
                {
                    walkForwards = true;
                }
                else
                {
                    walkForwards = false;
                }
            }
            else if (angle >= 90f && angle <= 180f || angle <= -90f && angle >= -180f)
            {
                if (facingRight == true)
                {
                    FlipAnim();
                }

                if (move < 0)
                {
                    walkForwards = true;
                }
                else
                {
                    walkForwards = false;
                }
            }
        }
        else if (gunDrawn == false)
        {
            shootRight.enabled = false;
            shootLeft.enabled = false;
            spriteRenderRight.enabled = false;
            spriteRenderLeft.enabled = false;
            //print(spriteRenderRight.enabled + " and " + spriteRenderLeft.enabled);
            //Function call for flipping orientation based on direction of character
            if (move > 0 && !facingRight && (grounded != false))
            {
                FlipAnim();
            }
            else if (move < 0 && facingRight && (grounded != false))
            {
                FlipAnim();
            }
        }
    }

    public void FlipAnim()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /*
    public Vector3 angleDirection(float angleInDegrees, bool angleGlobal)
    {
        if(!angleGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 90, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    */
    }