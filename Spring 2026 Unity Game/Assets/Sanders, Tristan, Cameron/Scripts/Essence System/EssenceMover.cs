using UnityEngine;

public class EssenceMover : MonoBehaviour
{
    [SerializeField] private float force;

    private void OnTriggerStay2D(Collider2D collision)
    {
        ApplyForce(collision);
    }

    private void ApplyForce(Collider2D other)
    {
        Vector2 forceDir = -transform.up;
        other.attachedRigidbody.AddForce(forceDir * force,ForceMode2D.Force);
    }
}
