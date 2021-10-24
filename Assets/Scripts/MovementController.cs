using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick Joystick;
    public float speed = 2f;
    private Vector3 velocityVector = Vector3.zero;
    private Rigidbody rb;
    public float maxVelocityChange = 4f;
    public float tiltAmount = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float _xMovementInput = Joystick.Horizontal;
        float _zMovementInput = Joystick.Vertical;

        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movmentVertical = transform.forward * _zMovementInput;

        Vector3 _movementVelocityVector = (_movementHorizontal + _movmentVertical).normalized * speed;

        Move(_movementVelocityVector);
        transform.rotation = Quaternion.Euler(Joystick.Vertical * speed * tiltAmount, 0, -Joystick.Horizontal * speed * tiltAmount);

    }

    void Move(Vector3 vector)
    {
        velocityVector = vector;    
    }

    private void FixedUpdate()
    {
        if(velocityVector!=Vector3.zero)
        {
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = velocityVector - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;
            rb.AddForce(velocityChange, ForceMode.Acceleration);

        }
    }
}
