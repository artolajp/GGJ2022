using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    [SerializeField] private float speed = 1.0f;
    
    private bool _actioning;
    private bool _lastActioning;
    
    private Rigidbody2D _rigidbody2D;

    private bool _moving;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (!_moving) {
            //_rigidbody2D.velocity = Vector2.zero;
        } else {
            _moving = false;
        }
        if (!_lastActioning && _actioning) {
            _actioning = false;
        }
        _lastActioning = false;
    }

    public void Move(Vector2 direction) {
        _moving = true;
        _rigidbody2D.velocity = new Vector2 (direction.normalized.x , direction.normalized.y) * speed;
    }

    public void Action() {
        _lastActioning = true;
        if (!_actioning) {
            _actioning = true;
            Debug.Log("ActionPlayer");
        } 
    }
}
