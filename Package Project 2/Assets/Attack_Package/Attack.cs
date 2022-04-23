using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public AnimationClip animation;
    public bool overrideMovement;
    public int damage;
    public float weight;
    public AudioClip sound;
}
