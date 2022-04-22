using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseExample : MonoBehaviour
{
    [SerializeField]
    GameObject sparkles;
    public void CreateSparks(GameObject player)
    {
        GameObject i_sparks = Instantiate(sparkles, player.transform);
        Destroy(i_sparks, 5);
    }
}
