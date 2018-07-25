using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Player Objects")]
    public GameObject player;
    public Rigidbody playerrb;
    public GameObject playerCam;
    [Header("Grapple Tool Objects")]
    public GameObject hook;
    private Rigidbody hookrb;
    public GameObject hookHolster;
    private Vector3 originalHookScale;
    public LineRenderer rope;
    [Header("Grapple Interactable Objects")]
    public GameObject hookedObject;
    public GameObject grabbedObject;
    public GameObject swingObject;

    [Header("Speed Values")]
    public float hookTravelSpeed;
    public float playerTravelSpeed;
    [Header("Grapple Distance Values")]
    public float maxDistance;
    public float currentDistance;
    [Header("Player Climb Values")]
    public float climbUp;
    public float climbForward;

    [Header("Grapple State Bools")]
    public bool isFired = false;
    public bool isHooked = false;
    public bool isGrabbed = false;
    public bool isSwinging = false;
    public bool isGrounded;



    void Start ()
    {
        rope = hook.GetComponent<LineRenderer>();
        hookrb = hook.GetComponent<Rigidbody>();
        originalHookScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
	
    void Update ()
    {
        CheckGround();

//launch grapple
        //if not fired, fire.
        if (Input.GetMouseButtonDown(0) && isFired == false)
            isFired = true;

        //if is fired, reset position and fire.
        if (Input.GetMouseButtonDown(0) && isFired)
        {
            RetractHook();
            isFired = true;
        }

        //reset hook position.
        if (Input.GetMouseButtonDown(1))
        {
            RetractHook();
        }


        //while fired.
        if (isFired)
        {
            //set line renderer vertices
            rope.SetVertexCount(2);
            rope.SetPosition(0, hookHolster.transform.position);
            rope.SetPosition(1, hook.transform.position);

            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if (currentDistance >= maxDistance)
                RetractHook();
        }


        if (isFired && isHooked == false && isGrabbed == false && isSwinging == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxDistance))
            {
                Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * maxDistance, Color.red);
                hook.transform.position = Vector3.MoveTowards(hook.transform.position, hit.point, hookTravelSpeed * Time.deltaTime);
            }
            else
            {
                hook.transform.position = Vector3.MoveTowards(hook.transform.position, playerCam.transform.position + playerCam.transform.forward * (maxDistance + 1), hookTravelSpeed * Time.deltaTime);
            }
        }


        //hooked  
        if (isHooked && isFired)
        {
            hook.transform.parent = hookedObject.transform;

            this.GetComponent<Rigidbody>().useGravity = false;

            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            if (distanceToHook < 2.5f)
            {
                if (isGrounded == false)
                {
                    this.transform.Translate(Vector3.up * Time.deltaTime * (hookedObject.GetComponent<Collider>().bounds.size.y + climbUp));
                    this.transform.Translate(Vector3.forward * Time.deltaTime * climbForward);
                }

                //if (transform.position.y < hookedObject.GetComponent<Collider>().bounds.size.y)
                //    playerrb.AddForce(transform.forward * -climbForward);

                StartCoroutine("Climb");
            }
        }
        else
        {
            hook.transform.parent = hookHolster.transform;
            this.GetComponent<Rigidbody>().useGravity = true;
        }


        //pull in object
        if (isGrabbed)
        {
            grabbedObject.transform.parent = hook.transform;
            hook.transform.localScale = originalHookScale;

            hook.transform.position = Vector3.MoveTowards(hook.transform.position, hookHolster.transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHolster = Vector3.Distance(hook.transform.position, hookHolster.transform.position);

            if (distanceToHolster < 0.5f)
            {
                Destroy(grabbedObject);
                grabbedObject = null;
                isGrabbed = false;
                isFired = false;
                rope.SetVertexCount(0);
                hook.transform.position = hookHolster.transform.position;
            }
        }
    }

    IEnumerator Climb()
    {
        yield return new WaitForSeconds(0.1f);
        RetractHook();
    }

    void RetractHook()
    {
        hook.transform.rotation = hookHolster.transform.rotation;
        hook.transform.position = hookHolster.transform.position;
        hook.transform.parent = hookHolster.transform;
        hook.transform.localScale = originalHookScale;
        isFired = false;
        isHooked = false;
        isSwinging = false;

        rope.SetVertexCount(0);
    }

    void CheckGround()
    {
        RaycastHit hit;
        float distance = 1f;

        Debug.DrawRay(transform.position, Vector3.down, Color.blue);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
