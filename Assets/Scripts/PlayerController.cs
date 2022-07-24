using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public InputReader InputReader { get; private set; }

    private Rigidbody2D _rigidbody;
    private Vector2 _direction = Vector2.down;

    public float speed = 5f;

    [Header("Sprites")] public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer _activeSpriteRenderer;

    private BombController _bombController;
    
    public delegate void OnDeadEvent(PlayerController playerController);
    
    public OnDeadEvent onDead;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _bombController = GetComponentInChildren<BombController>();
        if (_bombController == null)
        {
            Debug.Log("_bombController is null");
        }
        
        _activeSpriteRenderer = spriteRendererDown;
        SetInitialDirection(Vector2.zero, _activeSpriteRenderer);
        
        GameManager.Instance.RegisterPlayer(this);
    }

    private void OnEnable()
    {
        InputReader.ActionEvent += PerformAction;
    }

    private void OnDisable()
    {
        InputReader.ActionEvent -= PerformAction;
    }

    private void PerformAction()
    {
        Debug.Log("PerformAction");
        _bombController.TryPlaceBomb();
    }

    private void Update()
    {
        SetDirection(InputReader.MovementValue);
    }
    
    private void FixedUpdate()
    {
        Vector2 position = _rigidbody.position;
        Vector2 translation = _direction * (speed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(position + translation);
    }

    private void SetInitialDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        _direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        _activeSpriteRenderer = spriteRenderer;
        _activeSpriteRenderer.idle = _direction == Vector2.zero;
    }

    private void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;

        if (_direction == Vector2.up)
        {
            _activeSpriteRenderer = spriteRendererUp;
        }
        else if (_direction == Vector2.down)
        {
            _activeSpriteRenderer = spriteRendererDown;
        }
        
        if (_direction == Vector2.left)
        {
            _activeSpriteRenderer = spriteRendererLeft;
        }
        else if (_direction == Vector2.right)
        {
            _activeSpriteRenderer = spriteRendererRight;
        }

        _activeSpriteRenderer.enabled = true;
        _activeSpriteRenderer.idle = _direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        onDead?.Invoke(this);
    }

    public void AddSpeed()
    {
        speed++;
    }

    public void AddBomb()
    {
       _bombController.AddBomb();
    }

    public void AddExplosionRadius()
    {
        _bombController.AddExplosionRadius();
    }
}