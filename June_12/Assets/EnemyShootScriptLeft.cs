using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootScriptLeft : MonoBehaviour
{
    public int gunDamage = 1;

    public float fireRate = .25f;
    public float fireRateObstruct = 1f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public float speed = 5f;
    public float effectSpawnRate = 10;
    public float lifeTime = 1f;
    public static float angle;
    public float aimOff = 3;
    public bool gunFired = false;
    public bool playerWasCaught = false;

    bool facingRight = true;

    public Transform gunEnd;
    public Transform player;
    public Transform BulletTrailPrefab;
    public Transform hitPrefab;
    public Transform particlePrefab;
    public EnemyShootScript enemyShot;

    //public mcControllerScript playerScript;

    Animator anim;

    public WaitForSeconds shotDuration = new WaitForSeconds(.25f);
    //private AudioSource gunAudio;
    private LineRenderer laserLine;

    // Use this for initialization
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        anim = gameObject.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion qAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.localRotation = Quaternion.Slerp(transform.rotation, qAngle, speed * Time.deltaTime);

        anim.SetFloat("Angle", angle);
        anim.SetBool("GunFired", enemyShot.gunFired);

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
