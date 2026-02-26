using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    // For pure top-down, we usually only want height (Y)
    [SerializeField] private float _dist = 10f;
    [SerializeField] private float _smoothTime = 0.2f;

    private Vector3 _currentVelocity = Vector3.zero;

    private void Start()
    {
        // Force the camera to look straight down (90 degrees on X)
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void LateUpdate()
    {
        if (!_target) return;

        // Target position uses the player's X and Z, but stays at a fixed Y height
        Vector3 targetPosition = new Vector3(_target.position.x, _target.position.y, -_dist);

        // Smoothly move the camera
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);
    }
}
