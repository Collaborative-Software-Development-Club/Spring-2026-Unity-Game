using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 15f, -7f);
    [SerializeField] private float _smoothTime = 0.2f;

    private Vector3 _currentVelocity = Vector3.zero;

    private void LateUpdate()
    {
        if (!_target) return;

        // Calculate the ideal position based on the player and offset
        Vector3 targetPosition = _target.position + _offset;

        // Smoothly move the camera to that position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);

        // Ensure the camera is always looking at the player
        transform.LookAt(_target.position);
    }
}