using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    //Find players, add them to the list
    //Select nearest and set to target

    [SerializeField]
    private float visConeAngle;
    [SerializeField]
    private float visDistance;
    [SerializeField]
    private bool seeThroughWalls;
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private LayerMask ignore;
    private List<GameObject> targets;
    private Transform destination;
    private float h;
    private float o;

    void getPlayers()
    {
        targets = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    void checkVision()
    {
        foreach(GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (Vector3.Angle(target.transform.position - transform.position, transform.forward) < visConeAngle && dist < visDistance)
            {
                if (!seeThroughWalls)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(head.transform.position, target.transform.position, out hit, visDistance, ~ignore))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            Debug.Log("I see you!");
                        }
                    }
                }
                else
                {
                    Debug.Log("I see you!");
                }

                Debug.Log("Raycasting to player");
            }
        }
    }

    private void Update()
    {
        getPlayers();
        checkVision();
    }
    private void OnDrawGizmos()
    {
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
        Gizmos.DrawLine(head.transform.position, head.transform.position + worldVector);    }
}
