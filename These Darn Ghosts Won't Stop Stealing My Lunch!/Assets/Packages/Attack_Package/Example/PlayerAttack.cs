using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    AnimatorOverrideController aoc;

    [SerializeField]
    GameObject weapon;
    damage weaponScript;

    public List<AtkPattern> attackPatterns;

    //Combo system: After attacking, combo decay appears, attack again before it hits 0 to progress to next hit in combo

    [SerializeField]
    private float comboTime = 0.6f;
    private float elapsed;
    private int combo = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = aoc;
        weaponScript = weapon.GetComponent<damage>();
    }

    void Update()
    {
        // If not playing an animation
        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == -1019929913)
        {
            weaponScript.dmg = 0;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // If combo timer hasn't run out, add 1 to combo!
                if (elapsed > 0)
                {
                    combo += 1;
                }
                AtkPattern atk = attackPatterns.Find(p => p.name == "MainAttack");
                if (combo%2 == 0)
                {
                    Debug.Log("attack 1");
                    aoc["Attack1"] = atk.attacks[0].animation;
                    weaponScript.dmg = atk.attacks[0].damage;
                    AudioSource.PlayClipAtPoint(atk.attacks[0].sound, transform.position);
                }
                if (combo%2 == 1)
                {
                    Debug.Log("attack 2");
                    aoc["Attack1"] = atk.attacks[1].animation;
                    weaponScript.dmg = atk.attacks[1].damage;
                    AudioSource.PlayClipAtPoint(atk.attacks[1].sound, transform.position);
                }
                anim.Play("Attack1");
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                AtkPattern atk = attackPatterns.Find(p => p.name == "Lunge");
                aoc["Attack1"] = atk.attacks[0].animation;
                weaponScript.dmg = atk.attacks[0].damage;
                AudioSource.PlayClipAtPoint(atk.attacks[0].sound, transform.position);
                anim.Play("Attack1");
            }
            else
            {
            }
        }
        else
        {
            // Reset combo time when swinging, start decay when animation ends!
            elapsed = comboTime;
        }
        elapsed -= Time.deltaTime;
        //Debug.Log($"Combo time: {elapsed} Combo counter: {combo}");
        if (elapsed <= 0)
        {
            combo = 0;
        }
    }
}
