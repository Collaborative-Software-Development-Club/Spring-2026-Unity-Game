using UnityEngine;

public class EssenceMover : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    private void OnTriggerStay(Collider other)
    {
        ApplyForce(other);
    }

    private void ApplyForce(Collider other)
    {
        other.attachedRigidbody.AddForce(CoordConverter.ConvertXYToXZ(force), ForceMode.Force);
    }
}
