using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public AnimationClip animation;
    public bool overrideMovement;
    public float damage;
    public float weight;
    public AudioClip sound;
}
