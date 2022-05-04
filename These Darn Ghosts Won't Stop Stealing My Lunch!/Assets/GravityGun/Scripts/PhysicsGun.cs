using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PhysicsGun : MonoBehaviour
{

    [SerializeField] LayerMask PickUpLayer;
    [SerializeField] LayerMask ObjectCollisionLayer;
    [SerializeField] Transform ObjectPos;
    [SerializeField] Transform Cam;
    [SerializeField] float Range;
    [SerializeField] float ObjectSmoothRate;
    [SerializeField] float SpinForce;
    [SerializeField] float ThrowForce;
    [SerializeField] float ObjectDistance = 5;
    [SerializeField] Transform GunPoint;
    [SerializeField] int LaserDivisions = 10;

    private GameObject HeldObject;
    private Rigidbody HeldObjectRB;
    private bool HoldingObject = false;
    private LineRenderer LR;

    Vector3 SpinVector;

    private void Start()
    {
        LR = GetComponent<LineRenderer>();
        LR.positionCount = 0;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && !HoldingObject)
            PickUp();
        else if ((Input.GetKeyDown(KeyCode.Mouse0) && HoldingObject))
            Release();
        else if ((Input.GetKeyDown(KeyCode.Mouse1) && HoldingObject))
            Throw();
        else if ((Input.GetKeyDown(KeyCode.Q) && HoldingObject))
            Freeze();

        if (HoldingObject)
            DetectNearSurfaces();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0;
        }
       
        if(HoldingObject)
            UpdateHeldObject();

        ObjectDistance += Input.mouseScrollDelta.y;

        ObjectDistance = Mathf.Clamp(ObjectDistance, 5, Range);

    }

    private void LateUpdate()
    {
        if (HoldingObject)
            UpdateLine();
    }

    void UpdateLine()
    {
        
        LR.SetPosition(0, GunPoint.position);

        Vector3 gunToObject = HeldObject.transform.position - GunPoint.position;
        Vector3 objectToTarget = HeldObject.transform.position - ObjectPos.position;

        LR.SetPosition(LaserDivisions - 1, HeldObject.transform.position);
        for (int i = 1; i < LaserDivisions - 1; i++)
        {
            float t = (float)(i / (float)(LaserDivisions - 1));
            Vector3 p1 = GunPoint.position + gunToObject * t;
            Vector3 p2 = ObjectPos.transform.position + objectToTarget * t;
            LR.SetPosition(i, Vector3.Lerp(p1, p2, t));
        }
    }

    void DetectNearSurfaces()
    {
        RaycastHit hit;

        if (Physics.Raycast(Cam.position, Cam.forward, out hit, ObjectDistance, ObjectCollisionLayer))
        {
            ObjectPos.position = hit.point + hit.normal;
            if(Input.mouseScrollDelta.y != 0)
            ObjectDistance = Vector3.Distance(ObjectPos.position,Cam.position) - 5;
        }
        else
            ObjectPos.position =  Cam.position + Cam.forward * ObjectDistance;

    }

    void UpdateHeldObject()
    {
        HeldObject.transform.position = Vector3.Slerp(HeldObject.transform.position, ObjectPos.position, ObjectSmoothRate * Time.deltaTime);
        LR.positionCount = LaserDivisions;

    }


    void PickUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(Cam.position, Cam.forward, out hit, Range, PickUpLayer))
        {
            HeldObject = hit.transform.gameObject;

            if (HeldObject.GetComponent<Rigidbody>() != null)
                HeldObjectRB = HeldObject.GetComponent<Rigidbody>();
            else
                HeldObjectRB = HeldObject.AddComponent<Rigidbody>();

            HeldObjectRB.velocity = Vector3.zero;

            SpinVector = new Vector3(Random.value * SpinForce, Random.value * SpinForce, Random.value * SpinForce);
            HeldObjectRB.AddTorque(SpinVector,ForceMode.Force);


            HeldObjectRB.useGravity = false;
            HoldingObject = true;

        }
       
    }

    void Release()
    {

        LR.positionCount = 0;


        HeldObjectRB.useGravity = true;
        HoldingObject = false;

        HeldObject = null;
        HeldObjectRB = null;


    }
    void Throw()
    {

        HeldObjectRB.AddForce(ThrowForce * Cam.transform.forward, ForceMode.Impulse);

        LR.positionCount = 0;


        HeldObjectRB.useGravity = true;
        HoldingObject = false;

        HeldObject = null;
        //Destroy(HeldObjectRB);
        HeldObjectRB = null;
    }

    void Freeze()
    {
        HeldObjectRB.isKinematic = !HeldObjectRB.isKinematic;

        if (!HeldObjectRB.isKinematic)
        {
            SpinVector = new Vector3((Random.value * 2 - 1) * SpinForce, (Random.value * 2 - 1) * SpinForce, Random.value * SpinForce);
            HeldObjectRB.AddTorque(SpinVector, ForceMode.Force);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(Cam.position, Cam.forward * ObjectDistance);
    }
}
