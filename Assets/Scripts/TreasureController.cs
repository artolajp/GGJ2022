using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Object = UnityEngine.Object;

public class TreasureController : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _isTreasure;
    private PlayerController _currentPlayer;
    private SpriteRenderer _renderer;
    
    [SerializeField] private int treasureScoreForSecond = 10;
    [SerializeField] private int curseScoreForSecond = 10;
    [SerializeField] private float curseTime = 10;
    [SerializeField] private float treasureTime = 10;
    
    [SerializeField] private Color treasureColor = Color.green;
    [SerializeField] private Color curseColor = Color.magenta;
    private List<PlayerController> _targets;
    [SerializeField] private float _pushForce = 400.0f;
    public PlayerController AttachedPlayer => _currentPlayer;
    public bool IsTreasure => _isTreasure;
    public bool IsCurse => !_isTreasure;

    public void Initialize(GameManager gameManager, bool isTreasure = true)
    {
        _gameManager = gameManager;
        _isTreasure = isTreasure;
        _targets = new List<PlayerController>();
    }

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Invoke(nameof(InvertTreasure), treasureTime);
    }        

    public void InvertTreasure()
    {
        _isTreasure = !_isTreasure;
        _renderer.color = _isTreasure ? treasureColor : curseColor;
        Invoke(nameof(InvertTreasure),_isTreasure ? treasureTime : curseTime);
    }

    private void FixedUpdate()
    {
        if (!_currentPlayer) return;
        
        float score = _isTreasure ? treasureScoreForSecond : curseScoreForSecond;
        _gameManager.ScorePlayer( score * Time.deltaTime, _currentPlayer);
    }

    public void AttachPlayer(PlayerController playerController)
    {
        _currentPlayer = playerController;

        foreach (PlayerController target in _targets)
        {
            if (target == _currentPlayer) return;

            target.Push(transform.position, _pushForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<PlayerController>();
        if (target)
        {
            _targets.Add(target);
        }
        
        if (_currentPlayer != null) return;
        
        var player = other.GetComponent<PlayerController>();
        if (player)
        {
            AttachPlayer(player);
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

    private void Update()
    {
        if (!_currentPlayer) return;
        
        Vector3 newPosition = _currentPlayer.transform.position;
        newPosition.z = -1;
        transform.position = newPosition;
    }
    
}
