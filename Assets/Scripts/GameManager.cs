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
            _playerControllers.Add(Instantiate(playerPrefab));
        }

        _inputController = Instantiate(_inputControllerPrefab);
        _inputController.Initialize(this);
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
}
