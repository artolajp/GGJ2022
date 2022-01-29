using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _isTreasure;
    private PlayerController _currentPlayer;
    [SerializeField] private int treasureScoreForSecond = 10;
    [SerializeField] private int curseScoreForSecond = 10;

    public void Initialize(GameManager gameManager, bool isTreasure = true)
    {
        _gameManager = gameManager;
        _isTreasure = isTreasure;
    }

    public void InvertTreasure()
    {
        _isTreasure = !_isTreasure;
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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_currentPlayer != null) return;
        
        var player = col.GetComponent<PlayerController>();
        if (player)
        {
            AttachPlayer(player);
        }
    }

    private void Update()
    {
        if (_currentPlayer)
        {
            Vector3 newPosition = _currentPlayer.transform.position;
            newPosition.z = -1;
            transform.position = newPosition;
        }
    }
}
