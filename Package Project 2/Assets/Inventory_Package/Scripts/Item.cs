using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite image;
    public bool stackable;
    public int stackSize;
    public bool usable;
    public bool droppable;
    public string verb;
    public UnityEvent use;
}
