using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _sprintSpeed = 12f;
    [SerializeField] private float _rotationSpeed = 15f;

    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private SpriteRenderer _spriteRenderer; // Drag your sprite child here

    private Rigidbody _rb;
    private Vector2 _frameInput;
    private bool _isSprinting;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += (dir) => _frameInput = dir;
        _inputReader.SprintEvent += (sprinting) => _isSprinting = sprinting;
    }

    private void FixedUpdate()
    {
        float speed = _isSprinting ? _sprintSpeed : _walkSpeed;
        Vector3 moveDir = new Vector3(_frameInput.x, 0, _frameInput.y).normalized;

        _rb.linearVelocity = new Vector3(moveDir.x * speed, _rb.linearVelocity.y, moveDir.z * speed);

        HandleSpriteDirection();
    }

    private void HandleSpriteDirection()
    {
        if (_frameInput.x == 0) return;

        // Flip the sprite based on horizontal movement
        // This keeps the sprite "upright" but changes the facing direction
        _spriteRenderer.flipX = _frameInput.x < 0;
    }
}