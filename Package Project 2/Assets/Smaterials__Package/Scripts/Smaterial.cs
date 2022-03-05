using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smaterial : MonoBehaviour
{
    [Tooltip("The material of this gameObject")]
    public string material;
    [Tooltip("Play the sound from the point of contact, leave unchecked to play from the object's centre")]
    public bool playFromContact = false;

    [SerializeField, Tooltip("Minimum time between sounds")]
    private float frequency = 0.1f;
    [HideInInspector]
    public bool canPlay = true;
    [HideInInspector]
    public float velocity;
    private bool collide = false;

    private bool cr1 = false;
    private bool cr2 = false;
    private Rigidbody rb;
    

    private ObjectSounds os;

    private ContactPoint[] contacts = new ContactPoint[8];
    private int contactNum = 0;
    private int deltaContacts = 0;

    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>())
            rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
        collide = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        // Detect a change in the number of contacts (so every contact plays sound)
        deltaContacts = collision.GetContacts(contacts);
        if (deltaContacts != contactNum)
        {
            contactNum = deltaContacts;
            collide = true;
            
            if (collision.gameObject.GetComponent<Smaterial>() && collision.gameObject.GetComponent<Smaterial>().canPlay)
            {
                Smaterial other = collision.gameObject.GetComponent<Smaterial>();
                string otherMat = other.material;
                string mat = material;

                os = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<ObjectSounds>();
                if (playFromContact == true)
                {
                    os.constructSound(mat, otherMat, collision.GetContact(collision.contactCount - 1).point, gameObject);
                }
                else
                {
                    os.constructSound(mat, otherMat, transform.position, gameObject);
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (!canPlay && !cr1)
        {
            StartCoroutine(coolDown());
        }
        if (collide == false && rb)
        {
            velocity = rb.velocity.magnitude;
        }
        if(collide == true && !cr2)
        {
            StartCoroutine(deBounce());
        }
    }

    IEnumerator coolDown()
    {
        cr1 = true;
        yield return new WaitForSeconds(frequency);
        canPlay = true;
        cr1 = false;
    }

    IEnumerator deBounce()
    {
        cr2 = true;
        //yield return new WaitUntil(() => deltaContacts < contactNum);
        yield return new WaitForFixedUpdate();
        collide = false;
        cr2 = false;
    }
}
