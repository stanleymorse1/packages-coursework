using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    //Patrol behaviour
    //When losing player, check left, check right, then patrol in a 4 by 4 area 4m in front of where you were when you lost them
    //If not found within a short time range, resume a large patrol
    //Damage script
    //Different speeds for aggro and not aggro
    //Swap targets if: New target breaks previous LOS, new target deals enough damage
    //Identify if damage came from current target, if not, switch targets after a certain amount
    //Retreat/cover behaviour as bonus if you have time?

    [SerializeField]
    private float visConeAngle = 45;
    [SerializeField]
    private float visDistance = 20;
    public bool xRay;
    public bool facePlayer;
    [SerializeField]
    private float facePlayerSpd = 5;

    // Set to 0 to disable
    [SerializeField]
    private float meleeRange;
    public UnityEvent melee;
    [SerializeField]
    private float shootRange;
    public UnityEvent shoot;

    [SerializeField]
    private float strafeFrequency = 2.5f;
    [SerializeField]
    private float patrolRange = 10;
    [SerializeField]
    private float patrolFrequency = 6;
    [SerializeField]
    private bool fixedPatrol;
    public bool returnToPost;
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private LayerMask ignore;

    private List<GameObject> targets;
    private Transform focus;

    private NavMeshAgent agent;
    private float stopDist;
    private bool strafing;
    private bool patrolling;
    private float strafeSpd;
    private Vector3 post;

    private float h;
    private float o;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stopDist = agent.stoppingDistance;
        post = transform.position;
        getPlayers();
    }

    private void Update()
    {
        //If agent has signt
        if (checkVision())
        {
            float dist = (Vector3.Distance(focus.position, transform.position));

            // If the player is within stopping distance or the agent is tacticool, turn to face them
            if (dist <= agent.stoppingDistance && checkVision() || facePlayer && checkVision())
            {
                var targetRot = Quaternion.LookRotation(Vector3.Scale(focus.position - transform.position, new Vector3(1, 0, 1)), Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, facePlayerSpd * Time.deltaTime);
            }
            // Strafe if strafing (simple)
            if (strafing)
                agent.Move(strafeSpd * transform.right * Time.deltaTime);

            // If in melee range, run melee function
            if(dist < meleeRange)
                melee.Invoke();

            // If in shooting range, run shoot function
            if (dist < shootRange && dist > meleeRange)
                shoot.Invoke();

            // If the agent cannot see the player, go all the way to their last seen position, otherwise distance to protect the NHS
            agent.stoppingDistance = stopDist;
        }
        else
        {
            agent.stoppingDistance = 0;
        }
    }

    void getPlayers()
    {
        targets = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    bool checkVision()
    {
        // Check  all players in the game (objects tagged "Player")
        foreach(GameObject target in targets)
        {
            if (!xRay)
            {
                float dist = Vector3.Distance(transform.position, target.transform.position);
                // Are they within the vis cone?
                if (Vector3.Angle(target.transform.position - head.transform.position, transform.forward) < visConeAngle && dist < visDistance)
                {
                    // If the agent cannot see through walls, will it be able to spot the player?
                    RaycastHit hit;
                    Debug.DrawRay(head.transform.position, target.transform.position - head.transform.position);
                    if (Physics.Raycast(head.transform.position, target.transform.position - head.transform.position, out hit, visDistance, ~ignore))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            // Execute chase code
                            pathToPoint(hit.collider.transform);
                            return true;
                        }
                    }
                }
            }
            // If xRay mode is on, the AI automatically knows where the player is
            else
            {
                pathToPoint(target.transform);
                return true;
            }

        }
        if (returnToPost && Vector3.Distance(transform.position, post) > patrolRange && agent.remainingDistance <= 0.1f)
        {
            pathToPoint(post + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange)));
        }
        else
        {
            if (!patrolling && agent.remainingDistance <= 0.1f)
                StartCoroutine(patrol());
        }

        return false;
    }

    void pathToPoint(Transform dest)
    {
        agent.SetDestination(dest.position);
        focus = dest;
        if (strafing == false)
        { 
            strafing = true;StartCoroutine(strafeWalk()); // Two for one package deal, disgusting
        }
    }
    // Overload, fancy shit
    void pathToPoint(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    IEnumerator strafeWalk()
    {
        yield return new WaitForSeconds(Random.Range(strafeFrequency / 2, strafeFrequency));
        strafeSpd = Random.Range(-3, 6);// Set the strafe vector in here to make it more erratic
        strafing = false;
    }

    IEnumerator patrol()
    {
        Vector3 destination;
        patrolling = true;
        if (fixedPatrol)
        {
            destination = post + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
        }
        else
        {
            destination = new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
        }
        pathToPoint(destination);
        yield return new WaitForSeconds(Random.Range(patrolFrequency / 3, patrolFrequency));
        patrolling = false;
    }

    private void OnDrawGizmos()
    {
        // Fancy gizmo doohickeys using trigangle math or some shit

        h = visDistance / Mathf.Cos(visConeAngle * Mathf.Deg2Rad);
        o = h * Mathf.Tan(visConeAngle * Mathf.Deg2Rad);

        Vector3 worldVector;
        Gizmos.color = Color.red;
        worldVector =  head.transform.TransformVector(new Vector3(o, 0, visDistance));
        Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);
        worldVector = head.transform.TransformVector(new Vector3(-o, 0, visDistance));
        Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);
        worldVector = head.transform.TransformVector(new Vector3(0, o, visDistance));
        Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);
        worldVector = head.transform.TransformVector(new Vector3(0, -o, visDistance));
        Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);

        if (agent && agent.hasPath)
        {
            Gizmos.DrawCube(agent.destination, new Vector3(0.1f, 1f, 0.1f));
        }
    }
}
