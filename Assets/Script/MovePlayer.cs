using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    Rigidbody rb;

    float horizontal;
    float vertical;

    Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        //freeze rigidbody otherwise Player falls over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    //get input
    void MyInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        //ground check 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        SpeedControl();
    }

    //Nhung gi lien quan toi Vat Ly thi bo vao FixUpdate
    private void FixedUpdate()
    {
        MovePerson();
    }

    private void MovePerson()
    {
        //caculate movement direction
        moveDir = orientation.forward * vertical + orientation.right * horizontal;

        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if need
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
