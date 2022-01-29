using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{    
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _jumpForce = 350;
    [SerializeField] private Vector2 _jumpWallForce = new Vector2(250,250);
    
    private bool _actioning;
    private bool _lastActioning;
    
    private bool _actioningJump;
    private bool _lastActioningJump;
    
    private Rigidbody2D _rigidbody2D;

    private bool _moving;
    private bool _onFloor;
    private bool _onLeftWall;
    private bool _onRightWall;
    private bool _inputEnable;
    public float Score { get; set; }
    public int PlayerNumber { get; protected set; }

    private List<PlayerController> _targets;

    private GameManager _gameManager;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Initialize(int playerNumber, GameManager gameManager)
    {
        _inputEnable = true;
        Score = 0;
        PlayerNumber = playerNumber;
        _targets = new List<PlayerController>();
        _gameManager = gameManager;
    }

    private void Update()
    {
        var floorCast = Physics2D.Raycast(transform.position,  Vector3.down, 0.52f,LayerMask.GetMask("Floor"));
        _onFloor = floorCast.point.y > 0;
        var rightWallCast = Physics2D.Raycast(transform.position,  Vector3.right, 0.52f,LayerMask.GetMask("Floor"));
        _onRightWall = rightWallCast.point.y > 0;
        var leftWallCast = Physics2D.Raycast(transform.position,  Vector3.left, 0.52f,LayerMask.GetMask("Floor"));
        _onLeftWall = leftWallCast.point.y > 0;
    }

    private void FixedUpdate() {
        if (!_moving )
        {
            float breakSpeed = _onFloor ? 0.5f : 0.95f;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * breakSpeed, _rigidbody2D.velocity.y);
        } else if(_inputEnable){
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
        if (!_inputEnable) return;
        
        _moving = true;
        _rigidbody2D.velocity = Vector2.right * direction.normalized.x * _speed + Vector2.up * _rigidbody2D.velocity.y;
    }

    public void Action() {
        _lastActioning = true;

        if (!_inputEnable) return;
        
        if (!_actioning) {
            _actioning = true;
            Attack();
        } 
    }

    private void Attack()
    {
        foreach (PlayerController player in _targets)
        {
            _gameManager.Attack(this, player);
        }
    }

    public void Jump()
    {
        _lastActioningJump = true;
        if (!_inputEnable) return;
        
        if (!_actioningJump)
        {
            if (_onFloor)
            {
                _actioningJump = true;
                _rigidbody2D.AddForce(new Vector2(0, _jumpForce));
            }
            else if (_onRightWall)
            {
                _actioningJump = true;
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.AddForce(_jumpWallForce * new Vector2(-1, 1));
                DisableInputs();
            }
            else if (_onLeftWall)
            {
                _actioningJump = true;
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.AddForce(_jumpWallForce * new Vector2(1, 1));
                DisableInputs();
            }
        }
    }

    public void DisableInputs()
    {
        _inputEnable = false;
        Invoke(nameof(EnableInputs), 0.3f);
    }
    
    public void EnableInputs()
    {
        _inputEnable = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<PlayerController>();
        if (target)
        {
            _targets.Add(target);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        var target = other.GetComponent<PlayerController>();
        if (target)
        {
            _targets.Remove(target);
        }
    }

    public void Push(Vector3 origin, float pushForce)
    {
        Vector3 normalizedDirection = (transform.position-origin).normalized;
        _rigidbody2D.AddForce(normalizedDirection * pushForce);
    }
}
