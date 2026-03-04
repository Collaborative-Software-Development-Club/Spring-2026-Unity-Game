using UnityEngine;

public class EssenceMover : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Oh yes");
        ApplyForce(collision);
    }

    private void ApplyForce(Collider2D other)
    {
        other.attachedRigidbody.AddForce(force,ForceMode2D.Force);
    }
}
