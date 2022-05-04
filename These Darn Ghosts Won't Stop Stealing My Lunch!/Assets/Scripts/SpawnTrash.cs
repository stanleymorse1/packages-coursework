using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrash : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] prefabs;
    public float binLimit = 15;
    private Collider[] inBin;
    bool True = false;
    private BoxCollider trigger;

    private void Start()
    {
        trigger = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        inBin = Physics.OverlapBox(transform.position + trigger.center, trigger.size/2, transform.rotation);
        if (True == false && inBin.Length < binLimit)
        {
            StartCoroutine(spawn());
        }
        Debug.Log(inBin.Length);
    }
    IEnumerator spawn()
    {
        True = true;
        int rand = Random.Range(0, prefabs.Length);
        GameObject i_trash = Instantiate(prefabs[rand], spawnPoint.position, Quaternion.identity);
        i_trash.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-100, 100), -100, Random.Range(-100, 100)));
        yield return new WaitForSeconds(1);
        True = false;
    }
}
