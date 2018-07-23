using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
    
    float speed = 50f;

    [Header("Movement Stats")]
    public float walkSpeed = 10f;
    public float sprintSpeed = 15f;
    public float crouchSpeed = 5f;

    public float jumpHeight = 1f;

    [Header("Ground Detection")]
    public bool checkForGround;
    [SerializeField]
    private LayerMask GroundLayers;
    public float inAirSpeedMultiplier = 0.02f;

    Rigidbody RB;
    Vector3 direction;

    void Start()
    {
        RB = this.GetComponent<Rigidbody>();
    }

    void Update()
    {

        Speed();
        Move();

    }

    void Speed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    void Move()
    {
        if (CheckForGround())
        {
            direction = transform.rotation * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 newVel = direction * speed;
            RB.velocity = Vector3.MoveTowards(RB.velocity, newVel, 1);
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            direction = transform.rotation * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 newVel = direction * speed;
            RB.velocity = RB.velocity + newVel * inAirSpeedMultiplier;
        }
    }

    bool CheckForGround()
    {
        if (!checkForGround)
        {
            return true;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.01f, GroundLayers))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }

    void Jump()
    {
        RB.velocity = RB.velocity + (Vector3.up * jumpHeight);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
    }

}
