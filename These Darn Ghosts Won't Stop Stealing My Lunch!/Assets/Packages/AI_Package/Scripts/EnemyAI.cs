using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEditor;

public class EnemyAI : MonoBehaviour
{
    //          TODO:
    // Target breakable objects blocking the way if aggro
    // Debug
    // Retreat/cover behaviour as bonus if you have time?
    // Swap targets if: New target breaks previous LOS, new target deals enough damage (super bonus time)
    // Identify if damage came from current target, if not, switch targets after a certain amount



    //          BUGS:
    // AI starts pathing before patrol path has finished generating (when path is recalculated)
    // Grace period not working
    // Test AI max path dist
    // AI sometimes loses ability to path to player? Investigate

    [SerializeField]
    float visConeAngle = 45;
    [SerializeField]
    private float visDistance = 20;
    public bool xRay;
    public bool facePlayer;
    [SerializeField]
    float facePlayerSpd = 5;

    [SerializeField]
    float maxTrackDist;
    [SerializeField]
    float gracePeriod = 2.5f;
    [SerializeField]
    float searchRad;

    // Set to 0 to disable
    [SerializeField]
    float minAtkDelay = 0.5f;
    [SerializeField]
    float maxAtkDelay = 1;

    public TransformEvent attack;

    [SerializeField]
    float strafeFrequency = 2.5f;
    [SerializeField]
    float patrolSpeed = 1.1f;
    [SerializeField]
    float patrolRange = 10;
    [SerializeField]
    float patrolFrequency = 6;
    [SerializeField]
    bool fixedPatrol;
    public bool returnToPost;
    [SerializeField]
    GameObject head;
    [SerializeField]
    LayerMask ignore;

    List<GameObject> targets;
    Transform focus;

    NavMeshAgent agent;
    float stopDist;
    float igrace;
    bool strafing;
    bool patrolling;
    bool atkcd;
    bool search;
    bool aggro;
    float spd;
    float strafeSpd;
    Vector3 post;

    // Trigonometry stuff
    float h;
    float o;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spd = agent.speed;
        stopDist = agent.stoppingDistance;
        post = transform.position;
        getPlayers(); // Multiplayer compatability!
    }

    private void Update()
    {
        // If agent has sight, handle facing player and 
        if (checkVision() && focus/*&&(maxTrackDist > 0 && RemainingDistance(agent.path.corners) < maxTrackDist)*/)
        {
            Debug.Log("Chasing player");
            StopCoroutine(patrol());
            StopCoroutine(Investigate());
            float dist = (Vector3.Distance(focus.position, transform.position));

            // If the player is within stopping distance or the agent is tacticool, turn to face them
            if (dist <= agent.stoppingDistance || facePlayer)
            {
                var targetRot = Quaternion.LookRotation(Vector3.Scale(focus.position - transform.position, new Vector3(1, 0, 1)), Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, facePlayerSpd * Time.deltaTime);
            }
            // Strafe if strafing (simple)
            if (strafing)
                agent.Move(strafeSpd * transform.right * Time.deltaTime);
            if (!atkcd)
                StartCoroutine(attackcd());
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
        Debug.Log(targets[0]);
    }

    // Check the vision cone and line of sight for players, return true if agent can see a player, and path to them

    //TODO: countdown timer that ticks down after loss of sight, only report false AFTER the timer is up! spotting player resets timer
    bool checkVision()
    {
        Debug.Log("Checking vision");
        // Check  all players in the game (objects tagged "Player")
        foreach (GameObject target in targets)
        {
            if (!xRay)
            {
                float dist = Vector3.Distance(transform.position, target.transform.position);
                // Are they within the vis cone?
                if (Vector3.Angle(target.transform.position - head.transform.position, transform.forward) < visConeAngle && dist < visDistance)
                {
                    RaycastHit hit;
                    // Debug.DrawRay(head.transform.position, target.transform.position - head.transform.position);
                    if (Physics.Raycast(head.transform.position, target.transform.position - head.transform.position, out hit, visDistance, ~ignore))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            // Execute chase code, unless target outside of max distance and max distance is enabled.
                            if (!(maxTrackDist > 0 && RemainingDistance(agent.path.corners) > maxTrackDist)/* && igrace > 0*/)
                            {
                                pathToPoint(hit.collider.transform);
                            }
                            return true;
                        }
                    }
                }
            }
            // If xRay mode is on, the AI automatically knows where the player is
            else
            {
                if (!(maxTrackDist > 0 && RemainingDistance(agent.path.corners) > maxTrackDist))
                {
                    pathToPoint(target.transform);
                }
                return true;
            }

        }
        igrace -= Time.deltaTime;
        if (igrace > 0)
        {
            Debug.Log(igrace);
            return true;
        }
        else
        {
            lostContact();
            return false;
        }
    }

    // Path towards an enemy, strafing where necessary
    void pathToPoint(Transform dest)
    {
        igrace = gracePeriod;
        agent.speed = spd;
        aggro = true;
        agent.SetDestination(dest.position);
        focus = dest;
        // If not strafing and target is a straight shot, start strafing
        if (!strafing && agent.path.corners.Length == 2)
        {
            strafing = true;
            StartCoroutine(strafeWalk());
        }
    }
    // Overload, fancy shit
    void pathToPoint(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    // Called every frame while agent can't see player
    private void lostContact()
    {
        agent.stoppingDistance = 0;
        Debug.Log("lost contact");
        // If the agent is supposed to patrol a fixed distance from a point
        if (fixedPatrol && !returnToPost)
        {
            post = transform.position;
        }
        if (searchRad > 0 && agent.remainingDistance <= 0.6f && search == false && aggro == true)
        {
            StopCoroutine(Investigate());
            StopCoroutine(patrol());
            StartCoroutine(Investigate());
            search = true;
        }

        if (maxTrackDist <= 0)
        {
            if (!patrolling && agent.remainingDistance <= 0.1f)
                StartCoroutine(patrol());
        }
        if (aggro == false)
        {
            if (!patrolling)
            {
                StartCoroutine(patrol());
            }
        }
    }

    // Calculate the actual length of the path the agent has to travel
    public float RemainingDistance(Vector3[] points)
    {
        if (points.Length < 2) return 0;
        float distance = 0;
        for (int i = 0; i < points.Length - 1; i++)
            distance += Vector3.Distance(points[i], points[i + 1]);
        return distance;
    }

    // Apply fancy footwork
    IEnumerator strafeWalk()
    {
        yield return new WaitForSeconds(Random.Range(strafeFrequency / 2, strafeFrequency));
        strafeSpd = Mathf.Round(Random.Range(-1, 1));// Set the strafe vector in here to make it more erratic
        strafing = false;
    }

    // Procedural patrol algorithm
    IEnumerator patrol()
    {
        agent.speed = patrolSpeed;
        Vector3 destination;
        Vector3 randomPos = new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
        NavMeshPath path = new NavMeshPath();
        patrolling = true;
        if (fixedPatrol || (returnToPost && Vector3.Distance(transform.position, post) > patrolRange && !patrolling))
        {
            destination = post + randomPos;
        }
        else
        {
            destination = transform.position + randomPos;
        }

        // Calculate the path first to see if it is too long, if it is, cancel and retry the randomization
        agent.CalculatePath(destination, path);
        while (maxTrackDist > 0 && RemainingDistance(path.corners) > maxTrackDist) // WHILE LOOPS ARE DANGEROUS
        {
            Debug.Log("Patrol point is too far away! Recalculating...");
            randomPos = new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
            if (fixedPatrol)
            {
                destination = post + randomPos;
            }
            else
            {
                destination = transform.position + randomPos;
            }
            agent.CalculatePath(destination, path);
        }

        pathToPoint(destination);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => agent.remainingDistance <= 0.1f);
        yield return new WaitForSeconds(Random.Range(patrolFrequency / 3, patrolFrequency));
        patrolling = false;
    }

    // Handle cooldowns for attacks
    IEnumerator attackcd()
    {
        atkcd = true;
        //Debug.Log("attack cooldown");
        attack.Invoke(focus);
        yield return new WaitForSeconds(Random.Range(minAtkDelay, maxAtkDelay));
        atkcd = false;
    }

    // Investigate the target's last seen position more thoroughly
    IEnumerator Investigate()
    {
        Vector3 iPoint = transform.position + transform.forward * searchRad;
        Vector3 i;
        Vector3 f;
        Debug.Log("Searching nearby...");
        agent.ResetPath();
        NavMeshHit hit;
        agent.Raycast(iPoint, out hit);

        pathToPoint(hit.position);
        //yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => agent.remainingDistance <= 0.1f);
        Debug.Log("Rotating right");
        i = transform.forward;
        f = transform.right;
        yield return new WaitUntil(() => turn(i, f));
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Rotating left");
        i = transform.forward;
        f = -transform.forward;
        yield return new WaitUntil(() => turn(i, f));

        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        agent.Raycast(new Vector3(Random.Range(-searchRad, searchRad), 0, Random.Range(-searchRad, searchRad)), out hit);

        pathToPoint(hit.position);

        yield return new WaitUntil(() => agent.remainingDistance <= 0.1f);
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));

        search = false;
        aggro = false;
        Debug.Log("Giving up search");
        lostContact();
    }
    private bool turn(Vector3 initial, Vector3 final)
    {
        agent.updateRotation = false;

        Vector3 dir = Vector3.Slerp(transform.forward, final, 2 * Time.deltaTime);
        Quaternion targetRot = Quaternion.LookRotation(dir, transform.up);

        //Debug.Log(Vector3.Angle(transform.forward, final));
        if (Vector3.Angle(transform.forward, final) > 1)
        {
            transform.rotation = targetRot;
            return false;
        }
        else
        {
            agent.updateRotation = true;
            return true;
        }
    }
    //var targetRot = Quaternion.LookRotation(Vector3.Scale(focus.position - transform.position, new Vector3(1, 0, 1)), Vector3.up);
    //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, facePlayerSpd* Time.deltaTime);

    private void OnDrawGizmos()
    {
        // Fancy gizmo doohickeys using trigangle math or some shit
        if (Selection.Contains(gameObject))
        {
            h = visDistance / Mathf.Cos(visConeAngle * Mathf.Deg2Rad);
            o = h * Mathf.Tan(visConeAngle * Mathf.Deg2Rad);

            Vector3 worldVector;
            Gizmos.color = Color.red;
            worldVector = head.transform.TransformVector(new Vector3(o, 0, visDistance));
            Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);
            worldVector = head.transform.TransformVector(new Vector3(-o, 0, visDistance));
            Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);
            worldVector = head.transform.TransformVector(new Vector3(0, o, visDistance));
            Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);
            worldVector = head.transform.TransformVector(new Vector3(0, -o, visDistance));
            Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);
        }
        if (agent && agent.hasPath)
        {
            Gizmos.DrawCube(agent.destination, new Vector3(0.1f, 1f, 0.1f));
        }
    }
}