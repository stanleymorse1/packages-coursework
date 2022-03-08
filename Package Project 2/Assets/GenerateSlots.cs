using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class GenerateSlots : MonoBehaviour
{
    private CharInventory ci;
    [SerializeField]
    private GameObject player;

    void Update()
    {
        ci = player.GetComponent<CharInventory>();
        ci.slots = new List<GameObject>(ci.capacity);

        Debug.Log(ci.slots.Capacity);

        if (ci.slots.Count < ci.capacity)
        {
            GameObject i_slot = Instantiate(ci.pSlot, transform);
            ci.slots.Add(i_slot);
            Debug.Log($"{ci.slots.Count} slots out of {ci.capacity}");
        }
        else
        {
            ci.slots.RemoveAt(ci.slots.Capacity - 1);
            Destroy(ci.slots[ci.slots.Capacity - 1]);
        }
    }
}
