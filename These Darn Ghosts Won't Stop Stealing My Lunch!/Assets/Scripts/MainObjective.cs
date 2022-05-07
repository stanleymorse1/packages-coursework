using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainObjective : MonoBehaviour
{
    [SerializeField]
    GameObject display;
    public int snacks;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        display.GetComponent<Text>().text = $"Food items: {snacks}";

        if(Physics.CheckSphere(transform.position, 4))
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            snacks -= 1;
            other.gameObject.SendMessage("Retreat");
        }
    }
}
