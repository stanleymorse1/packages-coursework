using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    AnimatorOverrideController aoc;

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
    }

    void Update()
    {
        // If not playing an animation
        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == -1019929913)
        {
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
                }
                if (combo%2 == 1)
                {
                    Debug.Log("attack 2");
                    aoc["Attack1"] = atk.attacks[1].animation;
                }
                anim.Play("Attack1");

            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                AtkPattern atk = attackPatterns.Find(p => p.name == "Lunge");
                aoc["Attack1"] = atk.attacks[0].animation;
                anim.Play("Attack1");
            }
        }
        else
        {
            // Reset combo time when swinging, start decay when animation ends!
            elapsed = comboTime;
        }
        elapsed -= Time.deltaTime;
        Debug.Log($"Combo time: {elapsed} Combo counter: {combo}");
        if (elapsed <= 0)
        {
            combo = 0;
        }
    }
}
