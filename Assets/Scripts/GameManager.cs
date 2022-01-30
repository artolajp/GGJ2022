using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputController _inputControllerPrefab;
    private InputController _inputController;
    
    [SerializeField] private List<PlayerController> playerPrefabs;
    private List<PlayerController> _playerControllers;

    [SerializeField] private TreasureController _treasureControllerPrefab;
    private TreasureController _treasureController;
    
    [SerializeField] private List<Transform> playerSpawnPositions;
    [SerializeField] private int _playerCount = 4 ;

    [SerializeField] private UIController _uiController;

    [SerializeField] private float _targetScore = 200;
    [SerializeField] private float _matchDuration = 99;
    private float _currentMatchTime;

    [SerializeField] private GameObject winPanel;
    
    public int PlayerCount => _playerCount;

    private PlayerController _winPlayer;
    public bool IsPlaying => _winPlayer==null;

    private void Awake()
    {
        _playerControllers = new List<PlayerController>(_playerCount);
        for (int i = 0; i < _playerCount; i++)
        {
            PlayerController playerController = Instantiate(playerPrefabs[i],playerSpawnPositions[i].position,transform.rotation,transform);
            playerController.Initialize(i, this);
            _playerControllers.Add(playerController);
        }

        _inputController = Instantiate(_inputControllerPrefab);
        _inputController.Initialize(this);

        _treasureController = Instantiate(_treasureControllerPrefab);
        _treasureController.Initialize(this);

        _currentMatchTime = _matchDuration;
    }

    private void Update()
    {
        _uiController.Refresh(_playerControllers, _treasureController);
        _currentMatchTime -= Time.deltaTime;
        if (_currentMatchTime <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        PlayerController winner = _playerControllers[0];
        for (int i = 1; i < _playerControllers.Count; i++)
        {
            if (_playerControllers[i].Score > winner.Score)
            {
                winner = _playerControllers[i];
            }
        }

        _winPlayer = winner;
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
        if (IsPlaying)
        {
            player.Score = Mathf.Max(player.Score + score,0) ;
        }
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
