using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CharInventory : MonoBehaviour
{
    //Player gets near object, list of nearby objects is detected and returned
    //If look not required, select object closest to the spherecast hit, if spherecast hit nothing, then centre of player
    //Every time inventory Count changes, check if item picked up is stackable, if not append to end of list, if yes:
    //Divide number of that item by stacksize, create number of stacks related to result rounded up
    //If stack size is 0, stack forever

    //Push all items in slots higher than the item dropped down by one.
    //OR 

    public GameObject playerCamera;
    public GameObject invScreen;
    public GameObject pSlot;
    public GameObject pItem;
    public float dropOffset = 0.5f;

    public int capacity;
    public List<GameObject> Inventory = new List<GameObject>();
    
    [SerializeField]
    private KeyCode pickUpInput;
    [SerializeField]
    private KeyCode openInput;
    [SerializeField, Tooltip("Pickup radius, in a sphere around the centre of the character")]
    private float pickUpRange = 4;
    //[SerializeField, Tooltip("Do you have to be looking at object to pick it up? If not, distance to centre of screen is priority")]
    bool requireLookAt = true;
    [SerializeField, Tooltip("Thickness of sightline raycast")]
    float raySize = 0.3f;
    [SerializeField]
    private LayerMask ignore;

    public List<GameObject> slots;
    private RaycastHit hit;
    private Item selected;
    private bool invOpen;
    private Text debugList;

    void Start()
    {
        debugList = invScreen.transform.Find("Inventory/TextPanel/DebugList").GetComponent<Text>();
        invOpen = invScreen.transform.Find("Inventory").gameObject.activeSelf;
        Inventory.Capacity = capacity;
        slots = invScreen.GetComponent<GenerateSlots>().slots;
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
                        pickUpItem(selected);
                    }
                }
            }
        }
        else
        {
            checkRadius().GetValue(0);
        }
        if (invOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
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
            Debug.Log("Pressed E");
            invScreen.transform.Find("Inventory").gameObject.SetActive(!invOpen);
            invOpen = !invOpen;
            transform.GetComponent<ExamplePlayer>().enabled = !invOpen;
        }
    }

    void pickUpItem(Item item)
    {
        // If there is space in the inventory, add the object and disable it in the world.
        if (Inventory.Count < Inventory.Capacity)
        {
            Inventory.Add(item.gameObject);
            item.gameObject.SetActive(false);
            GameObject i_item = Instantiate(pItem, slots[Inventory.Count-1].transform);

            //If there is no image, display text always. If there is, display text on hover. // REMEMBER TO IMPLEMENT THIS THICKO
            i_item.transform.Find("Name").GetComponent<Text>().text = item.itemName;
            if(item.image != null)
            {
                i_item.GetComponent<Image>().sprite = item.image;
            }

            //In future, replace "LMB" and "RMB" with images, and if possible, images relating to custom inputs too
            string prompt = "";
            if(item.usable)
            {
                if (string.Concat(item.verb) == "")
                {
                    prompt += $"LMB: Use";
                }
                else
                {
                    prompt += $"LMB: {item.verb}";
                }
            }
            if(item.usable && item.droppable)
            {
                prompt += " | ";
            }
            if (item.droppable)
            {
                prompt += "RMB: Drop";
            }
            i_item.transform.Find("HoverPrompt/Prompt").GetComponent<Text>().text = prompt;
            i_item.GetComponent<ClickableObject>().item = item.gameObject;
            //updateInv();
        }
        else
        {
            Debug.Log("No free slots in suit inventory");
        }
    }

    public void updateInv()
    {
        //debugList.text = Inventory.ToString();
        for (int i = 0; i < Inventory.Capacity; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                if (slots[i + 1].transform.childCount != 0)
                {
                    Debug.Log("No item in slot! Moving next item up...");
                    slots[i + 1].GetComponentInChildren<ClickableObject>().transform.SetParent(slots[i].transform, false);
                }
            }
        }


        //DEBUG: Dropping the first item does not cause successors to move down
        //All items dropped after first drop the item before them in the list.

        //For each item in the array, check if slot before it is empty, move item to that slot, repeat.
        //This way the inventory items always move to the top left
    }

    void dropItem(GameObject item)
    {
        item.SetActive(true);
        item.transform.position = transform.position + transform.forward * dropOffset;
        Destroy(slots[Inventory.IndexOf(item)].GetComponentInChildren<ClickableObject>().gameObject);
        Inventory.Remove(item);
        Invoke("updateInv", Time.deltaTime);
        //When item is dropped, remember to shift all items back one in the inventory!
    }
}