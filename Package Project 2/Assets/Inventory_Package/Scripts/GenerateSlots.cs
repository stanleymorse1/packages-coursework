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

    private int childNum;


    private void Awake()
    {
        ci = player.GetComponent<CharInventory>();
        slots = new List<GameObject>(0);
    }

    void OnRenderObject()
    {
        //Something is destroying slots that should be in the list
        if (transform.childCount > ci.capacity)
        {
            Debug.Log("Removing excess slots!");
            //for (int i = transform.childCount - 1; i >=0; i--)
            //{
            //    DestroyImmediate(transform.GetChild(i).gameObject);
            //}
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
                slots.RemoveAt(slots.IndexOf(child.gameObject));
            }

        }
        //if (transform.childCount == 0)
        //{
        //    slots = new List<GameObject>(0);
        //}
        Debug.Log($"{slots.Count} slots out of {ci.capacity}");
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
