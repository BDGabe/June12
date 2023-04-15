using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mcControllerScript : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float maxSprint = 15f;
    public float jumpForce = 50f;
    public float mag = 0f;
    public float distToGround = 0;
    public float angle;
    public float buttonClickRate = 0.25f;
    public static float move;

    public Vector3 normal = Vector3.up;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public GameObject armOne;
    public GameObject armTwo;

    bool grounded = false;
    bool facingRight = true;
    bool gunDrawn = true;
    bool walkForwards = true;

    float groundRadius = 0.15f;
    float gunDrawnCount = 1;
    float nextClick;

    Rigidbody player;

    Animator anim;

    Collider collide;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collide = GetComponent<Collider>();

        //distToGround = collide.bounds.extents.y - collide.center.y;
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxis("Horizontal");

        grounded = Physics.CheckSphere(groundCheck.position, groundRadius, whatIsGround);
        mag = player.velocity.magnitude;

        anim.SetBool("GunDrawn", gunDrawn);
        anim.SetBool("Ground", grounded);
        anim.SetBool("WalkingForwards", walkForwards);
        anim.SetFloat("vSpeed", player.velocity.y);
        anim.SetFloat("Speed", player.velocity.magnitude); //Mathf.Abs(move)
        
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);

        //if statement checking if player is grounded which determines movement
        if (grounded != false)
        {
            if (move < 0)
            {
                //player.AddForce(maxSpeed * -1, 0, 0);
                player.velocity = new Vector2(maxSpeed * -1, player.velocity.y);
            }
            else if (move > 0)
            {
                //player.AddForce(maxSpeed, 0, 0);
                player.velocity = new Vector2(maxSpeed, player.velocity.y);
            }
        }

        if(Input.GetKey(KeyCode.Mouse1) && Time.time > nextClick)
        {
            nextClick = Time.time + buttonClickRate;

            gunDrawnCount = gunDrawnCount * -1;

            if(gunDrawnCount == 1)
            {
                gunDrawn = true;
            }
            else
            {
                gunDrawn = false;
            }
        }

        if(gunDrawn)
        {
            if (angle > 0f && angle < 90f || angle < 0f && angle > -90f)
            {
                if (facingRight == false)
                {
                    Flip();
                }

                if(move > 0)
                {
                    walkForwards = true;
                }
                else
                {
                    walkForwards = false;
                }
            }
            else if (angle > 90f && angle < 180f || angle < -90f && angle > -180f)
            {
                if (facingRight == true)
                {
                    Flip();
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
        else if (!gunDrawn)
        {
            //Function call for flipping orientation based on direction of character
            if (move > 0 && !facingRight && (grounded != false))
            {
                Flip();
            }
            else if (move < 0 && facingRight && (grounded != false))
            {
                Flip();
            }
            //sprint
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (grounded != false))
            {
                if (move > 0)
                {
                    //player.AddForce(maxSprint, 0, 0);
                    player.velocity = new Vector2(maxSprint, player.velocity.y);
                }
                else if (move < 0)
                {
                    //player.AddForce(maxSprint * -1, 0, 0);
                    player.velocity = new Vector2(maxSprint * -1, player.velocity.y);
                }
            }
        }

        //stops player movement completely when under max walk speed
        if ((mag < 1.5))
        {
            player.velocity = new Vector2(move * maxSpeed, player.velocity.y);
        }
    //}

    //private void Update()
    //{
        //AddForce for player jump
        if (grounded && (Input.GetButtonDown("Jump")))
        {
            anim.SetBool("Ground", false);
            player.AddForce(new Vector2(0, jumpForce));
        }
        else if (!grounded && Input.GetButtonUp("Jump"))
        {
            if (player.velocity.y > 0)
            {
                player.velocity = new Vector2(player.velocity.x, player.velocity.y * 0.5f);
            }
        }
    }

    //Function to flip sprite orientation when facing either left or right of screen.
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
