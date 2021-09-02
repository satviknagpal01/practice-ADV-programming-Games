using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movements : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody rigidbody = new Rigidbody();
    public Animator anim;
    public float smoothMovementTime = 0.1f;
    public float turnSpeed = 2f;
    private int abc = 1;
    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;
    


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
            
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMovementTime);
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
        velocity = transform.forward * speed * smoothInputMagnitude;
        if (velocity.magnitude > 0.1)
        {
            anim.SetBool("walk", true);
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed *= 2;
                anim.SetBool("sprint",true);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed /= 2;
                anim.SetBool("sprint",false);
            }
        }
        else if(velocity.magnitude<=0.1)
        {
            anim.SetBool("walk", false);
            anim.SetBool("sprint",false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("jump",true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("jump",false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            speed /= 2;
            if (abc > 4)
                abc = 1;
            anim.SetInteger("attack",abc);
            abc++;
        }if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            speed *= 2;
            anim.SetInteger("attack",0);
        }
    }
    private void FixedUpdate()
    {
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
    }
}