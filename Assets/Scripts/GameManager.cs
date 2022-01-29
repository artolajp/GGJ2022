using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputController _inputControllerPrefab;
    private InputController _inputController;
    
    [SerializeField] private PlayerController playerPrefab;
    private List<PlayerController> _playerControllers;

    [SerializeField] private TreasureController _treasureControllerPrefab;
    private TreasureController _treasureController;
    
    [SerializeField] private List<Transform> playerSpawnPositions;
    [SerializeField] private int _playerCount = 4 ;

    public int PlayerCount => _playerCount;

    private PlayerController _winPlayer;
    public bool IsPlaying => _winPlayer==null;

    private void Awake()
    {
        _playerControllers = new List<PlayerController>(_playerCount);
        for (int i = 0; i < _playerCount; i++)
        {
            PlayerController playerController = Instantiate(playerPrefab);
            playerController.Initialize(i, this);
            _playerControllers.Add(playerController);
        }

        _inputController = Instantiate(_inputControllerPrefab);
        _inputController.Initialize(this);

        _treasureController = Instantiate(_treasureControllerPrefab);
        _treasureController.Initialize(this);
    }

    public void MovePlayer(int playerNumber, Vector2 direction) {
        _playerControllers[playerNumber].Move(direction);

    }

    public void ActionPlayer(int playerNumber) {
        _playerControllers[playerNumber].Action();
    }
    
    public void JumpPlayer(int playerNumber) {
        _playerControllers[playerNumber].Jump();
    }

    public void ScorePlayer(float score, PlayerController player)
    {
        player.Score += score;
    }

    public void Attack(PlayerController player, PlayerController target)
    {
        if (_treasureController.IsTreasure && _treasureController.AttachedPlayer == target)
        {
            _treasureController.AttachPlayer(player);
        }else if (_treasureController.IsCurse && _treasureController.AttachedPlayer == player)
        {
            _treasureController.AttachPlayer(target);
        }
    }
}
