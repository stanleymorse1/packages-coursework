using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateSlots : MonoBehaviour
{
    //[HideInInspector]
    public List<GameObject> slots;
    private CharInventory ci;
    [SerializeField]
    private GameObject player;

    private void Start()
    {
        ci = player.GetComponent<CharInventory>();
        if (transform.childCount <= ci.capacity)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i));
            }
            slots = new List<GameObject>(0);
        }
    }
    void OnRenderObject()
    {
        //Switch this to a for loop you fucking idiot
        Debug.Log($"{slots.Count} slots out of {ci.capacity}");
        /*
        if (slots.Count > ci.capacity)
        {
            DestroyImmediate(slots[slots.Count - 1]);
            slots.RemoveAt(slots.Count - 1);
        }
        if (slots.Count < ci.capacity)
        {
            GameObject i_slot = Instantiate(ci.pSlot, transform);
            slots.Add(i_slot);
        }
        */
        if (slots.Count > ci.capacity)
        {
            for (int i = ci.capacity; i >= 0; i--)
            {
                DestroyImmediate(slots[i]);
                slots.RemoveAt(i);
            }
        }
        if (slots.Count < ci.capacity)
        {
            for (int i = 0; i < ci.capacity; i++)
            {
                GameObject i_slot = Instantiate(ci.pSlot, transform);
                slots.Add(i_slot);
            }
        }


    }
}
