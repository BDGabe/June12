using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootScript : MonoBehaviour
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

    float timeToSpawnEffect = 0f;


    public Transform gunEnd;
    public Transform player;
    public Transform BulletTrailPrefab;
    public Transform hitPrefab;
    public Transform particlePrefab;
    public LineOfSight enemySight;

    //public mcControllerScript playerScript;

    Animator anim;

    public WaitForSeconds shotDuration = new WaitForSeconds(.25f);
    //private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;

    // Use this for initialization
    void Start ()
    {
        laserLine = GetComponent<LineRenderer>();
        anim = gameObject.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        /*
        var targetPos = player.transform.position;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(targetPos.x - screenPoint.x, targetPos.y - screenPoint.y);
        angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
        */

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion qAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.localRotation = Quaternion.Slerp(transform.rotation, qAngle, speed * Time.deltaTime);

        anim.SetFloat("Angle", angle);
        anim.SetBool("GunFired", gunFired);

       if ((enemySight.playerSighted == true) && (enemySight.playerObstructed() == false) && (Time.time > nextFire))
       {
            //Debug.Log("Eff-ing See me bro One");
            nextFire = Time.time + Random.Range(fireRate, fireRate * 2);
            shoot();
            playerWasCaught = true;
       }
       else if((playerWasCaught == true) && (enemySight.playerObstructed() == true) && (Time.time > nextFire))
       {
            //Debug.Log("Eff-ing See me bro Two");
            nextFire = Time.time + Random.Range(fireRateObstruct, fireRateObstruct * 2);
            shoot();
       }

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

    private IEnumerator shotEffect()
    {
        //gunAudio.Play();

        //laserLine.enabled = true;
        gunFired = true;
        yield return shotDuration;
        //laserLine.enabled = false;
        gunFired = false;
    }

    void FlipAnim()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;
    }

    void shoot()
    {
        StartCoroutine(shotEffect());

        Vector3 shoot = transform.TransformDirection(Vector3.right);

        RaycastHit hit;

        laserLine.SetPosition(0, gunEnd.position);

        if (Physics.Raycast(gunEnd.position, shoot, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);

            Shootable health = hit.collider.GetComponent<Shootable>();

            if (health != null)
            {
                health.Damage(gunDamage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            laserLine.SetPosition(1, gunEnd.position + (shoot * weaponRange));
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPosition;
            Vector3 hitNorm;

            if (hit.collider == null)
            {
                hitPosition = (shoot * weaponRange);
                hitNorm = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPosition = hit.point;
                hitNorm = hit.normal;
            }

            Effect(hitPosition, hitNorm);
            timeToSpawnEffect = Time.time + 1 / (effectSpawnRate);
        }
    }

    void Effect(Vector3 hitPosition, Vector3 hitNormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab, gunEnd.position, gunEnd.rotation);
        LineRenderer line = trail.GetComponent<LineRenderer>();

        if (line != null)
        {
            line.SetPosition(0, gunEnd.position);
            line.SetPosition(1, hitPosition);
        }

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Instantiate(hitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.left, hitNormal));
            Instantiate(particlePrefab, hitPosition, Quaternion.FromToRotation(Vector3.up, hitNormal));
        }
        Destroy(trail.gameObject, lifeTime);
    }
}
