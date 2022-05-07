using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityDamage : MonoBehaviour
{
    Rigidbody rb;
    damage dmgScript;
    [SerializeField]
    float velocityMult = 1.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dmgScript = GetComponent<damage>();
    }
    private void FixedUpdate()
    {
        dmgScript.dmg = Mathf.RoundToInt(rb.velocity.magnitude * velocityMult);
    }
}
