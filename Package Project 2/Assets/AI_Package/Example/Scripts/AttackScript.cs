using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField]
    GameObject fx;
    [SerializeField]
    float range = 4;
    public void attack(Transform target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) < range)
        {
            Debug.Log("Attack recieved");
            GameObject i_sparks = Instantiate(fx, target.transform.position, target.transform.rotation);
            Destroy(i_sparks, 3);
        }
    }
}
