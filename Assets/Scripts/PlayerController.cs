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
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _mainSprite;
    
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
        _onFloor = IsRaycastWith(transform.position + Vector3.right * 0.3f, Vector3.down + Vector3.right * 0.3f, "Floor");
        _onFloor |= IsRaycastWith(transform.position + Vector3.left * 0.3f, Vector3.down - Vector3.left * 0.3f, "Floor");
        _onRightWall = IsRaycastWith(transform.position, Vector3.right, "Floor");
        _onRightWall |= IsRaycastWith(transform.position + Vector3.down * 0.3f, Vector3.right + Vector3.down * 0.3f, "Floor");
        _onLeftWall = IsRaycastWith(transform.position, Vector3.left, "Floor");
        _onLeftWall |= IsRaycastWith(transform.position + Vector3.down * 0.3f, Vector3.left + Vector3.down * 0.3f, "Floor");
        if (_onFloor)
        {
            _animator.SetBool("isJumping",false);
        }
    }

    private bool IsRaycastWith(Vector2 origin, Vector2 direction, string tag)
    {
        bool isRaycast = false;
        var casts = Physics2D.RaycastAll(origin,  direction, 0.6f);
        foreach (var hit2D in casts)
        {
            if(PlayerNumber== 0 && hit2D.collider.tag == tag) Debug.Log("Hit with floor");
            isRaycast |= hit2D.collider.tag == tag;
        }

        return isRaycast;
    }

    private void FixedUpdate() {
        if (!_moving )
        {
            float breakSpeed = _onFloor ? 0.5f : 0.95f;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * breakSpeed, _rigidbody2D.velocity.y);
        } else if(_inputEnable){
            _moving = false;            
            _animator.SetBool("isWalking",false);
        }
        
        if (!_lastActioning && _actioning) {
            _actioning = false;            
            _animator.SetBool("isAttaking",false);
        }
        _lastActioning = false;
        
        if (!_lastActioningJump && _actioningJump) {
            _actioningJump = false;
        }
        _lastActioningJump = false;
    }

    public void Move(Vector2 direction) {
        if (!_inputEnable) return;
        
        _animator.SetBool("isWalking",true);
        _moving = true;
        Vector2 velocity = new Vector2(direction.normalized.x * _speed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = velocity;

        _mainSprite.gameObject.transform.localScale = velocity.x > 0? Vector3.one: new Vector3(-1,1,1);
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
        _animator.SetBool("isAttaking",true);

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
                _animator.SetBool("isJumping",true);

            }
            else if (_onRightWall)
            {
                _actioningJump = true;
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.AddForce(_jumpWallForce * new Vector2(-1, 1));
                DisableInputs();
                _animator.SetBool("isJumping",true);

            }
            else if (_onLeftWall)
            {
                _actioningJump = true;
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.AddForce(_jumpWallForce * new Vector2(1, 1));
                DisableInputs();
                _animator.SetBool("isJumping",true);
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
        normalizedDirection.y = Mathf.Abs(normalizedDirection.y);
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(normalizedDirection * pushForce);

        DisableInputs();
    }
}
