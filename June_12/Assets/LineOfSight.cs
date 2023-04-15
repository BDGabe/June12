using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float lookRadius = 10f;

    Transform playerTarget;
    Rigidbody enemy;

    public bool playerSighted = false;
    //public static bool playerObstructed = true;
    //public static float angle;
    public float speed = 5f;
    public float sightRange = 100f;
    public float fieldOfView = 45f;
    public float maxDistance;

    //public Transform player;
    //public Transform eyeLine;
    public Transform lineOfSightEnd;

    //private LineRenderer laserLine;

    // Use this for initialization
    void Start ()
    {
        playerTarget = playerManager.instance.player.transform;

        enemy = GetComponentInParent<Rigidbody>();

        //laserLine = GetComponent<LineRenderer>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float distance = Vector3.Distance(playerTarget.position, lineOfSightEnd.position);

        if (distance <= lookRadius)
        {
            if(playerSeen())
            {
                playerSighted = true;
            }
            else
            {
                playerSighted = false;
            }
        }
    }

    public bool playerSeen ()
    {
        if (playerInFOV())
        {
            return (!playerObstructed());
        }
        else
        {
            return false;
        }
    }

    bool playerInFOV ()
    {
        Vector3 lineOfSightOne = lineOfSightEnd.position - (transform.position + new Vector3(lookRadius, 0, 0));
        Vector3 lineOfSightTwo = lineOfSightEnd.position - (transform.position - new Vector3(lookRadius, 0, 0));

        Debug.DrawLine(lineOfSightEnd.position, transform.position + new Vector3(lookRadius, 0, 0), Color.red);
        Debug.DrawLine(lineOfSightEnd.position, transform.position - new Vector3(lookRadius, 0, 0), Color.red);

        Vector3 direction = lineOfSightEnd.position - playerTarget.position;
        Debug.DrawLine(lineOfSightEnd.position, playerTarget.position, Color.yellow);

        if(playerTarget.position.x - transform.position.x >= 0)
        {
            float angle = Vector3.Angle(direction, lineOfSightOne);

            if ((angle < fieldOfView) && (Vector2.Distance(transform.position, playerTarget.position) >= maxDistance))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (playerTarget.position.x - transform.position.x < 0)
        {
            float angle = Vector3.Angle(direction, lineOfSightTwo);

            if ((angle < fieldOfView) && (Vector2.Distance(transform.position, playerTarget.position) >= maxDistance))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool playerObstructed ()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, playerTarget.position - transform.position, distanceToPlayer);
        //Debug.DrawRay(transform.position, playerTarget.position - transform.position, Color.blue);
        List<float> distances = new List<float>();

        foreach (RaycastHit hit in hits)
        {
            if(hit.transform.tag == "Enemy")
            {
                continue;
            }

            if(hit.transform.tag != "Player")
            {
                return true;
            }
        }

        return false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
