using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateSlots : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> slots;
    public GameObject slotsParent;
    private CharInventory ci;
    [SerializeField]
    private GameObject player;

    private void Start()
    {
        ci = player.GetComponent<CharInventory>();
        slots = new List<GameObject>(0);
    }

    void OnRenderObject()
    {
        if (slotsParent.transform.childCount > ci.capacity)
        {
            Debug.Log("Removing excess slots!");
            foreach (Transform child in slotsParent.transform)
            {
                DestroyImmediate(child.gameObject);
                slots.RemoveAt(slots.IndexOf(child.gameObject));
            }

        }

        //Debug.Log($"{slots.Count} slots out of {ci.capacity}");
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
                GameObject i_slot = Instantiate(ci.pSlot, slotsParent.transform);
                slots.Add(i_slot);
            }
        }


    }
}
