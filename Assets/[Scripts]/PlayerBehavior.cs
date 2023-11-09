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

    [SerializeField]
    float _jumpSpeedLimit;
    // Start is called before the first frame update

    [SerializeField]
    Transform _groundPoint;

    [SerializeField]
    float _jumpingPower = 20;

    [SerializeField]
    bool _isGrounded = false;

    float _airbornSpeedMultiplier = .6f;

    Joystick _leftJoystick;
    [SerializeField]
    [Range(0, 1)]
    float _treshold;

    [SerializeField]
    LayerMask _groundingLayers;

    Animator _animator;
    void Start()
    {
        if(GameObject.Find("OnScreenController"))
        {
            _leftJoystick = GameObject.Find("LeftController").GetComponent<Joystick>();
        }
      
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
        IsGrounded();

        if(_rigidbody.velocity.y > _jumpSpeedLimit)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeedLimit);
        }

        if (!_isGrounded)
        {
            if(_rigidbody.velocity.y >=0)
            {
                _animator.SetInteger("State", (int)AnimationState.JUMP);
            }
            else
            {
                _animator.SetInteger("State", (int)AnimationState.FALL);
            }
        }
        else
        {
            if(Mathf.Abs(_rigidbody.velocity.x) > 0.1f)
            {
                _animator.SetInteger("State", (int)AnimationState.WALK);
            }
            else
            {
                _animator.SetInteger("State", (int)AnimationState.IDLE);
            }
         
        }
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

        if (!_isGrounded)
        {
            applicableAcceleration *= _airbornSpeedMultiplier; //Airborne speed
        }



        Vector2 force = xMovementDirection * Vector2.right * applicableAcceleration;


        if(xMovementDirection < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (xMovementDirection > 0)
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

        if(_isGrounded && (Input.GetKeyDown(KeyCode.Space) || leftJoystickVerticalInput > _treshold))
        {
            _rigidbody.AddForce(Vector2.up * _jumpingPower, ForceMode2D.Impulse);
        }
    }

    void IsGrounded()
    {
        _isGrounded = Physics2D.CircleCast(_groundPoint.position, .1f, Vector2.down, .1f, _groundingLayers);


    }


}
