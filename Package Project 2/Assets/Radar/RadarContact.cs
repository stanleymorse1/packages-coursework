using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarContact : MonoBehaviour
{
    private GameObject player;
    [HideInInspector]
    public int index = -1;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player.GetComponent<RadarDisplay>())
        {
            player.GetComponent<RadarDisplay>().contacts.Add(gameObject);
        }
        else
        {
            Debug.Log("No radar display on player!");
            return;
        }
        
    }

    void Update()
    {
        //Change this to only ping when the player or contact position has changed since last frame
        if (player)
        {
            player.SendMessage("updPos", gameObject);
            Debug.Log("Updating contact pos");
        }
        else
        {
            Debug.Log("No player found! Have you set the player gameobject's tag?");
        }
    }
}
