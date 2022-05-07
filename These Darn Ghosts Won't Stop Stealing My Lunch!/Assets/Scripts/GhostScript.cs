using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScript : MonoBehaviour
{
    Transform target;
    [HideInInspector]
    public Transform grave;

    NavMeshAgent agent;

    bool hasFood = false;
    void Start()
    {
        target = GameObject.FindWithTag("Picnic").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.enabled)
        {
            if (hasFood == false)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                agent.SetDestination(grave.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFood && other.CompareTag("Finish"))
        {
            gameObject.layer = 2;
            agent.enabled = false;
            InvokeRepeating("sink", 0, Time.deltaTime);
            Destroy(gameObject, 2);
        }
    }

    public void Retreat()
    {
        gameObject.tag = "Enemy";
        gameObject.layer = 7;
        hasFood = true;
    }

    void sink()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -4, transform.position.z), 0.01f);
    }
}
