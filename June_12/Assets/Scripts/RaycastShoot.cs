using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public mcControllerScript playerScript;

    public int gunDamage = 1;

    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public float speed = 5f;
    public float lifeTime = 1f;
    public float camShakeAmount = 0.0f;
    public float camShakeLength = 0.0f;
    public static float angle;
    public float aimOff = 3;
    public bool gunFired = false;
    public float effectSpawnRate = 10;

    bool facingRight = true;

    float timeToSpawnEffect = 0f;

    public Transform gunEnd;
    public Transform BulletTrailPrefab;
    public Transform hitPrefab;
    public Transform particlePrefab;
    public Transform gameMast;

    //public mcControllerScript playerScript;

    Animator anim;
    CameraShake camShake;

    public WaitForSeconds shotDuration = new WaitForSeconds(.2f);
    //private AudioSource gunAudio;
    private LineRenderer laserLine;
    public float nextFire;

    // Use this for initialization
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        anim = gameObject.GetComponent<Animator>();

        camShake = GameMasterScript.gm.GetComponent<CameraShake>();
        if(camShake == null)
        {
            Debug.LogError("No CameraShake script in GameMaster.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float move = mcControllerScript.move;

        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);

        Quaternion qAngle = Quaternion.Euler(0, 0, angle - aimOff);

        transform.localRotation = qAngle;

        /* Alternative method ::
        Vector3 direction = Input.mousePosition - transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        */

        anim.SetFloat("Angle", angle);
        anim.SetBool("GunFired", gunFired);

        if (fireRate == 0)
        {
            if(Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + 0.25f;
                shoot(false);        
                //camera shake
                camShake.shake(camShakeAmount, camShakeLength);
            }
        }
        else if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            shoot(true);
            //camera shake
            camShake.shake(camShakeAmount, camShakeLength);
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

    private IEnumerator shotEffect(bool buttonHeld)
    {
        //gunAudio.Play();

        //laserLine.enabled = true;
        gunFired = true;
        if(buttonHeld == true)
        {
            yield return new WaitWhile(() => Input.GetButton("Fire1"));
            yield return shotDuration;
        }
        else
        {
            yield return shotDuration;
        }
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

    void shoot(bool buttonHeld)
    {
        StartCoroutine(shotEffect(buttonHeld));

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

        if(Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPosition;
            Vector3 hitNorm;
            
            if(hit.collider == null)
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
        if(BulletTrailPrefab != null)
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
}
