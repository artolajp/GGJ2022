using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    [SerializeField] private float speed = 1.0f;
    
    [SerializeField] private float _jumpForce = 50;
    
    private bool _actioning;
    private bool _lastActioning;
    
    private bool _actioningJump;
    private bool _lastActioningJump;
    
    private Rigidbody2D _rigidbody2D;

    private bool _moving;
    private bool _onFloor;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var other = Physics2D.Raycast(transform.position,  Vector3.down, 0.6f,LayerMask.GetMask("Floor"));
        _onFloor = other.point.y > 0;
    }

    private void FixedUpdate() {
        if (!_moving) {            
            _rigidbody2D.velocity = Vector2.up * _rigidbody2D.velocity.y;
        } else {
            _moving = false;
        }
        
        if (!_lastActioning && _actioning) {
            _actioning = false;
        }
        _lastActioning = false;
        
        if (!_lastActioningJump && _actioningJump) {
            _actioningJump = false;
        }
        _lastActioningJump = false;
    }

    public void Move(Vector2 direction) {
        _moving = true;
        _rigidbody2D.velocity = Vector2.right * direction.normalized.x * speed + Vector2.up * _rigidbody2D.velocity.y;
    }

    public void Action() {
        _lastActioning = true;
        if (!_actioning) {
            _actioning = true;
            Debug.Log("Action Player");
        } 
    }
    
    public void Jump()
    { 
        if (!_onFloor) return;

        _lastActioningJump = true;
        if (!_actioningJump)
        {
         _actioningJump = true;
         Debug.Log("Jump Player");
         _rigidbody2D.AddForce(new Vector2(0, _jumpForce));
        }
    }
}
