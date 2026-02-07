using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _mainCameraTransform;

    void Start() => _mainCameraTransform = Camera.main.transform;

    void LateUpdate()
    {
        // Makes the UI face the camera every frame
        transform.LookAt(transform.position + _mainCameraTransform.rotation * Vector3.forward,
                         _mainCameraTransform.rotation * Vector3.up);
    }
}