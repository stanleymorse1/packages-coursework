using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Strafe behaviour
    //Damage script
    //Identify if damage came from current target, if not, switch targets after a certain amount
    //Retreat/cover behaviour as bonus if you have time?

    [SerializeField]
    private float visConeAngle = 45;
    [SerializeField]
    private float visDistance = 20;
    public bool xRay;
    public bool facePlayer;
    [SerializeField]
    private float facePlayerSpd = 0.1f;
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private LayerMask ignore;
    private List<GameObject> targets;
    private NavMeshAgent agent;
    private Transform focus;
    private float stopDist;

    private float h;
    private float o;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stopDist = agent.stoppingDistance;
        getPlayers();
    }

    private void Update()
    {
        checkVision();
        // If the player is within stopping distance or the agent is tacticool, turn to face them
        if ((Vector3.Distance(focus.position, transform.position) <= agent.stoppingDistance && checkVision()) || facePlayer && checkVision())
        {
            var targetRot = Quaternion.LookRotation(Vector3.Scale(focus.position - transform.position, new Vector3(1, 0, 1)), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, facePlayerSpd * Time.deltaTime);
        }
        // If the agent cannot see the player, go all the way to their last seen position
        if (checkVision())
        {
            agent.stoppingDistance = stopDist;
        }
        else
        {
            agent.stoppingDistance = 0;
        }
        Debug.Log(agent.destination);
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
                if (Vector3.Angle(target.transform.position - transform.position, transform.forward) < visConeAngle && dist < visDistance)
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
        return false;
    }

    void pathToPoint(Transform dest)
    {
        agent.SetDestination(dest.position);
        focus = dest;
    }

    IEnumerator strafeWalk()
    {
        //Wait a random amount of seconds, pick a random direction to strafe in
        yield return new WaitForSeconds(3);
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
