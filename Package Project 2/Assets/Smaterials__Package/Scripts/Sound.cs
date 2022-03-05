using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [Tooltip("The two materials colliding which plays this sound, separated by a comma")]
    public string impactCombo;
    public AudioClip[] clip;
    public float minVolume = 0.13f;
    public float maxVolume = 1;
    public float spdVolMult = 0.2f;

    [HideInInspector]
    public AudioSource source;
}
