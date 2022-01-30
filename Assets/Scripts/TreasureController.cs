using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private SpriteRenderer _treasureSpriteRenderer;
    [SerializeField] private Color treasureColor = Color.green;
    [SerializeField] private Sprite treasureSprite;
    [SerializeField] private Color curseColor = Color.magenta;
    [SerializeField] private Sprite curseSprite;
    private List<PlayerController> _targets;
    [SerializeField] private float _pushForce = 400.0f;
    public Animator MaskAnimator => _gameManager.MaskAnimator;
    public PlayerController AttachedPlayer => _currentPlayer;
    public bool IsTreasure => _isTreasure;
    public bool IsCurse => !_isTreasure;

    [SerializeField] private AudioSource audioSource;

    public void Initialize(GameManager gameManager, bool isTreasure = true)
    {
        _gameManager = gameManager;
        _isTreasure = isTreasure;
        _targets = new List<PlayerController>();
    }

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Invoke(nameof(InvertTreasureMask), treasureTime-5);
        Invoke(nameof(InvertTreasure), treasureTime);
    }

    public void InvertTreasureMask() {
        MaskAnimator.SetTrigger("Activate");
    }

    public void InvertTreasure()
    {
        audioSource.Play();
        _isTreasure = !_isTreasure;
        _renderer.color = _isTreasure ? treasureColor : curseColor;
        _treasureSpriteRenderer.sprite = _isTreasure ? treasureSprite : curseSprite;
        Invoke(nameof(InvertTreasureMask), (_isTreasure ? treasureTime : curseTime) - 5);
        Invoke(nameof(InvertTreasure),_isTreasure ? treasureTime : curseTime);
    }

    private void FixedUpdate()
    {
        if (!_currentPlayer) return;
        
        float score = _isTreasure ? treasureScoreForSecond : -curseScoreForSecond;
        _gameManager.ScorePlayer( score * Time.deltaTime, _currentPlayer);
    }

    public void AttachPlayer(PlayerController playerController)
    {
        _currentPlayer = playerController;
        RepositionTreasure();
        foreach (PlayerController target in _targets)
        {
            if (target == _currentPlayer) continue;

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

        RepositionTreasure();
    }

    private void RepositionTreasure()
    {
        Vector3 newPosition = _currentPlayer.transform.position;
        transform.position = newPosition;
    }
}
