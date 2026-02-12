using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _sprintSpeed = 12f;

    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    // Reference the asset's script here
    [SerializeField] private Animator _animator;

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
        // Movement on X/Z plane for 3D physics
        Vector3 moveDir = new Vector3(_frameInput.x, 0, _frameInput.y).normalized;

        _rb.linearVelocity = new Vector3(moveDir.x * speed, _rb.linearVelocity.y, moveDir.z * speed);

        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (_animator == null) return;

        // Tell the animator if we are moving
        bool isMoving = _frameInput.magnitude > 0;
        _animator.SetBool("IsMoving", isMoving);

        if (!isMoving) return;

        // Determine direction integer for the asset's animator
        // 0: Down, 1: Up, 2: Right, 3: Left
        if (Mathf.Abs(_frameInput.x) > Mathf.Abs(_frameInput.y))
        {
            _animator.SetInteger("Direction", _frameInput.x > 0 ? 2 : 3);
        }
        else
        {
            _animator.SetInteger("Direction", _frameInput.y > 0 ? 1 : 0);
        }
    }
}