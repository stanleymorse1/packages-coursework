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
    private float swipeLimit = 1.5f;
    [SerializeField]
    private float lungeLimit = 20;

    [SerializeField]
    GameObject weapon;
    damage weaponScript;

    private Animator anim;
    AnimatorOverrideController aoc;

    public List<AtkPattern> attackPatterns;

    private void Start()
    {
        anim = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = aoc;
        weaponScript = weapon.GetComponent<damage>();
    }
    public void PickAttack(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        // If target is within my attack range but out of melee range
        if (dist <= lungeLimit && dist > swipeLimit && !AnimatorIsPlaying())
        {
            // Find the correct attack pattern
            AtkPattern atk = attackPatterns.Find(p => p.name == "Lunge");
            aoc["Attack1"] = atk.attacks[0].animation;
            weaponScript.dmg = atk.attacks[0].damage;
            anim.Play("Attack1");
            AudioSource.PlayClipAtPoint(atk.attacks[0].sound, transform.position);
            Debug.Log("Lunge attack");

        }
        else if (dist <= swipeLimit)
        {
            // Find the correct attack pattern
            AtkPattern atk = attackPatterns.Find(p => p.name == "MainAttack");
            aoc["Attack1"] = atk.attacks[0].animation;
            Debug.Log("Regular attack");

            //Weights system is a bit iffy, work on this mayhaps
            float weight = Random.Range(0f, 1f);

            foreach (Attack attack in atk.attacks)
            {
                //Debug.Log($"{weight} was calculated, {attack.weight} was required");
                if (weight < attack.weight)
                {
                    aoc["Attack1"] = attack.animation;
                    weaponScript.dmg = attack.damage;
                    AudioSource.PlayClipAtPoint(attack.sound, transform.position);
                    anim.Play("Attack1");
                }
            }
        }
    }
    bool AnimatorIsPlaying()
    { 
        return anim.GetCurrentAnimatorStateInfo(0).length > anim.GetCurrentAnimatorStateInfo(0).normalizedTime; 
    }
}
