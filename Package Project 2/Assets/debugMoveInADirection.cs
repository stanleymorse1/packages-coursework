using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugMoveInADirection : MonoBehaviour
{
    private Vector3 movement;
    bool cr = false;
    void Update()
    {
        if(cr == false)
            StartCoroutine(changeDirection());
        transform.Translate(movement*Time.deltaTime);
    }
    IEnumerator changeDirection()
    {
        cr = true;
        movement = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        yield return new WaitForSeconds(Random.Range(0, 10));
        cr = false;
    }
}
