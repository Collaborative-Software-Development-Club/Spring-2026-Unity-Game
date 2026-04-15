using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EssencePlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _sprintSpeed = 12f;

    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Animator _animator;

    [Header("Audio")]
    [SerializeField] private AudioSource _movementLoopAudioSource;
    [SerializeField] private AudioSource _movementOneShotAudioSource;
    [SerializeField] private AudioClip _movementStartClip;
    [SerializeField] private AudioClip _movementLoopClip;
    [SerializeField] private AudioClip _movementStopClip;
    [SerializeField][Range(0f, 1f)] private float _movementVolume = 1f;
    [SerializeField][Min(0f)] private float _movementThreshold = 0.05f;
    [SerializeField][Min(0f)] private float _loopStartOverlapSeconds = 0.02f;
    [SerializeField] private float _walkPitch = 1f;
    [SerializeField] private float _sprintPitch = 1.15f;

    private Rigidbody2D _rb;
    private Vector2 _frameInput;
    private bool _isSprinting;
    private bool _wasMoving;
    private Coroutine _startLoopCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EnsureMovementAudioSources();
    }

    private void OnEnable()
    {
        if (_inputReader == null)
        {
            return;
        }

        _inputReader.MoveEvent += HandleMove;
        _inputReader.SprintEvent += HandleSprint;
    }

    private void OnDisable()
    {
        if (_inputReader != null)
        {
            _inputReader.MoveEvent -= HandleMove;
            _inputReader.SprintEvent -= HandleSprint;
        }

        StopMovementAudio();
    }

    private void FixedUpdate()
    {
        float speed = _isSprinting ? _sprintSpeed : _walkSpeed;
        Vector2 moveDir = new Vector2(_frameInput.x, _frameInput.y).normalized;

        _rb.linearVelocity = new Vector2(moveDir.x, moveDir.y) * speed;

        bool isMoving = moveDir.sqrMagnitude > (_movementThreshold * _movementThreshold);
        HandleMovementAudio(isMoving);
        HandleAnimations();
    }

    private void HandleMove(Vector2 direction)
    {
        _frameInput = direction;
    }

    private void HandleSprint(bool sprinting)
    {
        _isSprinting = sprinting;
    }

    private void EnsureMovementAudioSources()
    {
        if (_movementLoopAudioSource == null && _movementLoopClip != null)
        {
            _movementLoopAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (_movementOneShotAudioSource == null && (_movementStartClip != null || _movementStopClip != null))
        {
            _movementOneShotAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (_movementLoopAudioSource != null)
        {
            _movementLoopAudioSource.playOnAwake = false;
            _movementLoopAudioSource.loop = true;
            _movementLoopAudioSource.spatialBlend = 0f;
            _movementLoopAudioSource.clip = _movementLoopClip;
            _movementLoopAudioSource.volume = _movementVolume;
        }

        if (_movementOneShotAudioSource != null)
        {
            _movementOneShotAudioSource.playOnAwake = false;
            _movementOneShotAudioSource.loop = false;
            _movementOneShotAudioSource.spatialBlend = 0f;
            _movementOneShotAudioSource.volume = _movementVolume;
        }
    }

    private void HandleMovementAudio(bool isMoving)
    {
        EnsureMovementAudioSources();

        if (_movementLoopAudioSource != null)
        {
            _movementLoopAudioSource.volume = _movementVolume;
            _movementLoopAudioSource.pitch = _isSprinting ? _sprintPitch : _walkPitch;
        }

        if (_movementOneShotAudioSource != null)
        {
            _movementOneShotAudioSource.volume = _movementVolume;
        }

        if (isMoving && !_wasMoving)
        {
            PlayMovementStart();
        }
        else if (isMoving)
        {
            EnsureMovementLoopIsRunning();
        }
        else if (!isMoving && _wasMoving)
        {
            PlayMovementStop();
        }

        _wasMoving = isMoving;
    }

    private void StopMovementAudio()
    {
        if (_startLoopCoroutine != null)
        {
            StopCoroutine(_startLoopCoroutine);
            _startLoopCoroutine = null;
        }

        if (_movementLoopAudioSource != null && _movementLoopAudioSource.isPlaying)
        {
            _movementLoopAudioSource.Stop();
        }
    }

    private void PlayMovementStart()
    {
        StopMovementAudio();

        if (_movementStartClip != null && _movementOneShotAudioSource != null)
        {
            _movementOneShotAudioSource.pitch = _isSprinting ? _sprintPitch : _walkPitch;
            _movementOneShotAudioSource.PlayOneShot(_movementStartClip, _movementVolume);
        }

        if (_movementLoopClip == null || _movementLoopAudioSource == null)
        {
            return;
        }

        float loopDelay = GetClipPlaybackDuration(_movementStartClip, _isSprinting ? _sprintPitch : _walkPitch) - _loopStartOverlapSeconds;
        if (loopDelay <= 0f)
        {
            StartMovementLoop();
            return;
        }

        _startLoopCoroutine = StartCoroutine(BeginMovementLoopAfterDelay(loopDelay));
    }

    private void PlayMovementStop()
    {
        StopMovementAudio();

        if (_movementStopClip != null && _movementOneShotAudioSource != null)
        {
            _movementOneShotAudioSource.pitch = _isSprinting ? _sprintPitch : _walkPitch;
            _movementOneShotAudioSource.PlayOneShot(_movementStopClip, _movementVolume);
        }
    }

    private IEnumerator BeginMovementLoopAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        _startLoopCoroutine = null;

        if (_wasMoving)
        {
            StartMovementLoop();
        }
    }

    private void StartMovementLoop()
    {
        if (_movementLoopAudioSource == null || _movementLoopClip == null)
        {
            return;
        }

        _movementLoopAudioSource.clip = _movementLoopClip;
        _movementLoopAudioSource.pitch = _isSprinting ? _sprintPitch : _walkPitch;

        if (!_movementLoopAudioSource.isPlaying)
        {
            _movementLoopAudioSource.Play();
        }
    }

    private void EnsureMovementLoopIsRunning()
    {
        if (_startLoopCoroutine != null)
        {
            return;
        }

        if (_movementLoopAudioSource == null || _movementLoopClip == null)
        {
            return;
        }

        if (_movementLoopAudioSource.isPlaying)
        {
            return;
        }

        if (_movementOneShotAudioSource != null && _movementOneShotAudioSource.isPlaying)
        {
            return;
        }

        StartMovementLoop();
    }

    private float GetClipPlaybackDuration(AudioClip clip, float pitch)
    {
        if (clip == null)
        {
            return 0f;
        }

        float safePitch = Mathf.Max(0.01f, Mathf.Abs(pitch));
        return clip.length / safePitch;
    }

    private void HandleAnimations()
    {
        if (_animator == null) return;

        bool isMoving = _frameInput.magnitude > 0;
        _animator.SetBool("IsMoving", isMoving);

        if (!isMoving) return;

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
