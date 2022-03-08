using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharInventory : MonoBehaviour
{
    //Player gets near object, list of nearby objects is detected and returned
    //If look not required, select object closest to the spherecast hit, if spherecast hit nothing, then centre of player
    //Every time inventory Count changes, check if item picked up is stackable, if not append to end of list, if yes:
    //Divide number of that item by stacksize, create number of stacks related to result rounded up
    //If stack size is 0, stack forever

    public GameObject playerCamera;
    public GameObject invScreen;

    [SerializeField]
    private int capacity;
    private List<GameObject> Inventory = new List<GameObject>();
    
    [SerializeField]
    private KeyCode pickUpInput;
    [SerializeField]
    private KeyCode openInput;
    [SerializeField, Tooltip("Pickup radius, in a sphere around the centre of the character")]
    private float pickUpRange = 4;
    [SerializeField, Tooltip("Do you have to be looking at object to pick it up? If not, distance to centre of screen is priority")]
    bool requireLookAt = false;
    [SerializeField, Tooltip("Thickness of sightline raycast")]
    float raySize = 0.3f;
    [SerializeField]
    private LayerMask ignore;

    private RaycastHit hit;
    private Item selected;
    private bool invOpen;
    private Text debugList;

    void Start()
    {
        debugList = invScreen.transform.Find("DebugList").GetComponent<Text>();
        invOpen = invScreen.activeSelf;
        Inventory.Capacity = capacity;
    }

    void Update()
    {
        openInv(Input.GetKeyDown(openInput));

        if (requireLookAt)
        {
            if (Physics.SphereCast(playerCamera.transform.position, raySize, playerCamera.transform.forward, out hit, pickUpRange - raySize, ~ignore))
            {
                if (requireLookAt && hit.collider.gameObject.GetComponent<Item>())
                {
                    selected = hit.collider.gameObject.GetComponent<Item>();
                    Debug.Log(selected.itemName);
                    if (Input.GetKeyDown(pickUpInput))
                    {
                        Debug.Log($"Picked up {selected.itemName}");
                        pickUpItem(selected.gameObject);
                    }
                }
            }
        }
        else
        {
            checkRadius().GetValue(0);
        }
    }

    Collider[] checkRadius()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, pickUpRange);

        return inRange;
    }

    void openInv(bool open)
    {
        if (open)
        {
            invScreen.SetActive(!invOpen);
            invOpen = !invOpen;
        }
    }

    void pickUpItem(GameObject obj)
    {
        // If there is space in the inventory, add the object and disable it in the world.
        if (Inventory.Count < Inventory.Capacity)
        {
            Inventory.Add(obj);
            obj.SetActive(false);
            //debugList.text += obj.GetComponent<Item>().itemName;
            updateInv();
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }

    void updateInv()
    {
        debugList.text = "";
        for (int i = 0; i < Inventory.Capacity; i++)
        {
            debugList.text += $"{Inventory[i].GetComponent<Item>().itemName}, ";
        }
    }
}