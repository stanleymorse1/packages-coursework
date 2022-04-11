using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    //Do range checks in this script
    //AI script passes in a transform and handles attack delays

    //TODO:

    //When player is at a medium range, disable strafe and override stop dist to close distance to player for melee!

    [SerializeField]
    private float rangedLimit = 20;
    [SerializeField]
    private float meleeLimit = 1.5f;


    private Animator anim;
    AnimatorOverrideController aoc;

    public List<AtkPattern> attackPatterns;

    private void Start()
    {
        anim = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = aoc;
        
    }
    public void PickAttack(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        // If target is within my attack range but out of melee range
        if (dist <= rangedLimit && dist > meleeLimit)
        {
            // Find the correct attack pattern
            AtkPattern atk = attackPatterns.Find(p => p.name == "Lunge");
            aoc["Attack1"] = atk.attacks[0].animation;
            anim.Play("Attack1");
            //anim.StopPlayback();
            Debug.Log("Ranged attack");

        }
        else if (dist <= meleeLimit)
        {
            // Find the correct attack pattern
            AtkPattern atk = attackPatterns.Find(p => p.name == "MainAttack");
            aoc["Attack1"] = atk.attacks[0].animation;

            Debug.Log("Melee attack");

            //Weights system is a bit iffy, work on this mayhaps
            float weight = Random.Range(0f, 1f);

            foreach (Attack attack in atk.attacks)
            {
                Debug.Log($"{weight} was calculated, {attack.weight} was required");
                if (weight < attack.weight)
                {
                    aoc["Attack1"] = attack.animation;
                    Debug.Log(attack.animation.name);
                    anim.Play("Attack1");
                    //anim.StopPlayback();
                }
            }
        }
    }
}
