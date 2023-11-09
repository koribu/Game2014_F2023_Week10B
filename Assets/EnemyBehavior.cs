using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    float _speed = 5;

    [SerializeField]
    Transform _groundCheckPoint;
    bool _isGrounded;
    [SerializeField]
    Transform _frontGroundPoint;
    bool _isThereGroundToStepOn;

    [SerializeField]
    Transform _frontObstaclePoint;
    bool _isThereAnyObstacle;

    [SerializeField]
    LayerMask _groundLayers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _isGrounded = Physics2D.Linecast(_groundCheckPoint.position, _groundCheckPoint.position + Vector3.down * .95f, _groundLayers);
        _isThereGroundToStepOn = Physics2D.Linecast(_groundCheckPoint.position, _frontGroundPoint.position,_groundLayers);
        _isThereAnyObstacle = Physics2D.Linecast(_groundCheckPoint.position,_frontObstaclePoint.position,_groundLayers);

        if(!_isThereGroundToStepOn || _isThereAnyObstacle)
        {
            ChangeDirection();
        }


        Move();
    }

    void Move()
    {
        transform.position += Vector3.left * transform.localScale.x * _speed;
    }

    void ChangeDirection()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(_groundCheckPoint.position, _groundCheckPoint.position + Vector3.down * .95f, Color.green, .001f);
        Debug.DrawLine(_groundCheckPoint.position, _frontGroundPoint.position, Color.green, .001f);
    }
}
