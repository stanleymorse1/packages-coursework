using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnChance;
    public GameObject enemy;
    public Transform spawnPoint;
    void spawn()
    {
        if(Random.Range(0f, 1f) <= spawnChance)
        {
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
