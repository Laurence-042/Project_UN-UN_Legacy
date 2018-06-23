using UnityEngine;
using System.Collections;


public class MechMovement : MonoBehaviour
{
    Transform player;               // Reference to the player's position.
    ThiefHealth playerHealth;      // Reference to the player's health.
    MechHealth mechHealth;        // Reference to this enemy's health.
    UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.
    Animator anim;
    [SerializeField] float stopDistance=7;//距离玩家的距离

    Ray shootRay = new Ray();                       
    RaycastHit shootHit;                            

    [SerializeField] Transform[] wayPoint;
    [SerializeField] int range = 10;//视野范围
    int currentWayPoint=0;

    public bool lostTarget = true;
    [SerializeField] bool gunMode = true;

    public bool moveing=true;
    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        playerHealth = player.GetComponent<ThiefHealth>();
        mechHealth = GetComponent<MechHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        // If the enemy and the player have health left...
        if (mechHealth.nowHP > 0 && playerHealth.currentHealth > 0&& moveing)
        {
            if (!lostTarget&&(Vector3.Distance(transform.position, player.position) > 1f||Vector3.Dot(transform.forward,player.position-transform.position)>0))
            {
                transform.LookAt(player.position, Vector3.up);
                nav.stoppingDistance = stopDistance;
                // ... set the destination of the nav mesh agent to the player.
                if (Vector3.Distance(transform.position, player.position) > nav.stoppingDistance)
                {
                    nav.SetDestination(player.position);
                    anim.SetBool("WalkForward", true);
                }
                else
                {
                    anim.SetBool("WalkForward", false);
                }
            }
            else
            {
                nav.stoppingDistance = 0f;
                if (Vector3.Distance(transform.position, wayPoint[currentWayPoint].position) > 0.5f)
                {
                    nav.SetDestination(wayPoint[currentWayPoint].position);
                    anim.SetBool("WalkForward", true);
                }
                else
                {
                    currentWayPoint++;
                    if (currentWayPoint == wayPoint.Length)
                    {
                        currentWayPoint = 0;
                    }
                }
                shootRay.origin = transform.position;
                shootRay.direction = transform.forward;

                if (gunMode)
                {
                    if (Physics.Raycast(shootRay, out shootHit, range))
                    {
                        if (shootHit.collider.tag == "Player")
                        {
                            lostTarget = false;
                        }
                    }
                    else
                    {
                        lostTarget = true;
                    }
                }

            }
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;

        }
    }
}


/*
 * // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRay.direction = player.position - transform.position;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, range))
            {
                if (shootHit.collider.tag != "Player")
                {
                    Debug.Log("Lost Target");
                    parent.GetComponent<MechMovement>().lostTarget = true;
                }
            }
            else
            {
                Shoot();
            }
 * */
