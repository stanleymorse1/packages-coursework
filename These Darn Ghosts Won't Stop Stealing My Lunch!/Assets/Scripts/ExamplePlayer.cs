using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayer : MonoBehaviour
{
    private CharacterController cc;
    public float speed;
    public float sens;
    public GameObject cam;
    public bool lockCam;
    public LayerMask ground;


    private void Start()
    {
        cc = GetComponent<CharacterController>();
        if (lockCam)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void Update()
    {
        Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.up * -5 + transform.right * Input.GetAxis("Horizontal");

        cc.Move(move * speed * Time.deltaTime);
        cam.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0) * -sens);
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
        if (Input.GetKeyDown(KeyCode.Space) && grounded())
        {

        }
    }

    bool grounded()
    {
        if (Physics.CheckBox(transform.position + new Vector3(0, -1, 0), new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, ground))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
