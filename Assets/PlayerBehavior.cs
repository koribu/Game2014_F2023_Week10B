using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    [SerializeField]
    float _accelerator = 100;
    [SerializeField]
    float _maxSpeed = 5;
    // Start is called before the first frame update

    [SerializeField]
    Transform _groundPoint;

    [SerializeField]
    float _jumpingPower = 20;

    bool _isGrounded = false;

    float _airbornSpeedMultiplier = .6f;

    Joystick _leftJoystick;
    [SerializeField]
    [Range(0, 1)]
    float _treshold;
    void Start()
    {
        if(GameObject.Find("OnScreenController"))
        {
            _leftJoystick = GameObject.Find("LeftController").GetComponent<Joystick>();
        }
      
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        //Jump
        Jump();
    }

    private void FixedUpdate()
    {
        //Movement
        Move();
    }

    private void Move()
    {
        float leftJoystickHorizontalInput = 0;
        if(_leftJoystick != null)
        {
            leftJoystickHorizontalInput = _leftJoystick.Horizontal;
        }

        float xMovementDirection = Input.GetAxisRaw("Horizontal") + leftJoystickHorizontalInput; // Get the direction of the movement


        float applicableAcceleration = _accelerator;

        if (!IsGrounded())
        {
            applicableAcceleration *= _airbornSpeedMultiplier; //Airborne speed
        }



        Vector2 force = xMovementDirection * Vector2.right * applicableAcceleration;


        if(xMovementDirection == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (xMovementDirection == 1)
        {
            transform.eulerAngles = Vector3.zero;
        }

        _rigidbody.AddForce(force);

        _rigidbody.velocity =  Vector2.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
    }

    private void Jump()
    {
        float leftJoystickVerticalInput = 0;
        if(_leftJoystick)
        {
            leftJoystickVerticalInput = _leftJoystick.Vertical;
        }

        if(IsGrounded() && (Input.GetKeyDown(KeyCode.Space) || leftJoystickVerticalInput > _treshold))
        {
            _rigidbody.AddForce(Vector2.up * _jumpingPower, ForceMode2D.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.CircleCast(_groundPoint.position, .1f, Vector2.down, .1f, LayerMask.GetMask("Ground"));


    }



}
