using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    //Do range checks in this script
    //Change AI script to pass in a transform and handle attack delays
    //Consider a class for each attack type
    [SerializeField]
    private float rangedLimit = 20;
    [SerializeField]
    private float meleeLimit = 1.5f;
    public void Attack(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        // If target is within my attack range but out of melee range
        if (dist <= rangedLimit && dist > meleeLimit)
        {
            Debug.Log("Ranged attack");
        }
        else if (dist <= meleeLimit)
        {
            float weight = Random.Range(0f, 1f);
            Debug.Log(weight);
        }
    }
}
