using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnChance;
    public GameObject enemy;
    public Transform spawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            spawn();
        }
    }

    void spawn()
    {
        if(Random.Range(0f, 1f) <= spawnChance)
        {
            GameObject I_Enemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            I_Enemy.GetComponent<GhostScript>().grave = transform;
        }
    }
}
