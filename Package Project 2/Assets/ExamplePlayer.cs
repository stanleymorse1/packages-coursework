using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayer : MonoBehaviour
{
    private CharacterController cc;
    public float speed;
    public float sens;
    public GameObject cam;
    public bool lockcam;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        if (lockcam)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void Update()
    {
        Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.up * -5 + transform.right * Input.GetAxis("Horizontal");

        cc.Move(move * speed * Time.deltaTime);
        cam.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0)*-sens);
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
    }
}
