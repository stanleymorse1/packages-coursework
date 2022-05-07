using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField]
    private string htag = "Enemy";

    public int dmg;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(htag) && dmg > 0)
        {
            Debug.Log("Hit enemy");
            other.gameObject.SendMessage("hurt", dmg);
            dmg = 0;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(htag) && dmg > 0)
        {
            Debug.Log("Hit enemy");
            other.gameObject.SendMessage("hurt", dmg);
            dmg = 0;
        }
    }
}
