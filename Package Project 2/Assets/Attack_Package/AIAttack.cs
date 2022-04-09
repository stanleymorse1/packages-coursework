using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    //Do range checks in this script
    //Change AI script to pass in a transform and handle attack delays
    //Consider a class for each attack type

    //When player is at a medium range, disable strafe and close distance to player for melee!

    [SerializeField]
    private float rangedLimit = 20;
    [SerializeField]
    private float meleeLimit = 1.5f;
    public void Attack(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        // If target is within my attack range but out of melee range
        Debug.Log("Attacking" + target.name);
        if (dist <= rangedLimit && dist > meleeLimit)
        {
            Debug.Log("Ranged attack");
        }
        else if (dist <= meleeLimit)
        {
            Debug.Log("Melee attack");
            float weight = Random.Range(0f, 1f);
            Debug.Log(weight);
        }
    }
}
