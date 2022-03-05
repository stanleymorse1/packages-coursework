using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectSounds : MonoBehaviour
{
    public Sound[] Sounds;
    private List<string> c_sound = new List<string>(2);
    private string soundCombo;

    private void Start()
    {
        for(int i = 0; i<= Sounds.Length-1; i++)
        {
            // Oh god
            string[] s = Sounds[i].impactCombo.Split(',');
            List<string> slist = s.ToList();
            slist.Sort();
            soundCombo = slist[0] + slist[1];
            Sounds[i].impactCombo = soundCombo.ToLower();
            // All of that just to standardise the bloody list
        }
    }

    public void constructSound(string material1,string material2, Vector3 point, GameObject caster)
    {
        Smaterial smat = caster.GetComponent<Smaterial>();
        c_sound.Add(material1);
        c_sound.Add(material2);
        c_sound.Sort();
        soundCombo = (c_sound[0] + c_sound[1]).ToLower();
        Debug.Log(soundCombo);
        smat.canPlay = false;
        float vol = smat.velocity;
        PlaySound(soundCombo, point, vol);
        c_sound.Clear();
    }

    public void PlaySound(string combo, Vector3 pos, float volume)
    {
        Sound s = Array.Find(Sounds, cmb => cmb.impactCombo == combo);
            
        

        if (s == null)
        {
            Debug.LogWarning($"No playable sound for the combo {combo}");
            return;
        }
        int r = UnityEngine.Random.Range(0, s.clip.Length);
        if (s.clip.Length == 0)
        {
            Debug.LogWarning($"Array size of audioclips for {s.impactCombo} is zero!");
            return;
        }
        if (s.clip[r] == null)
        {
            Debug.LogWarning($"Sound effect missing from slot {r} of {combo}");
            return;
        }
        if(volume < s.minVolume)
        {
            return;
        }
        else
        {
            volume = Mathf.Clamp(volume * s.spdVolMult, 0, s.maxVolume);
            Debug.Log($"Playing clip {r}");
            Debug.Log(volume);
            AudioSource.PlayClipAtPoint(s.clip[r], pos, volume);
        }

    }
}
