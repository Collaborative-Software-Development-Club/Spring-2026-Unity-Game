using UnityEngine;

public class EssenceSpell : SpellBehavior
{
    [Header("Spawn")]
    [SerializeField] GameObject essencePrefab;
    [SerializeField][Min(1)] int essenceCount = 15;
    [SerializeField][Min(0f)] float spawnRadius = 0.35f;

    [Header("Motion")]
    [SerializeField][Min(0f)] float launchSpeed = 8f;
    [SerializeField][Min(0f)] float bounciness = 1f;
    [SerializeField] bool zeroGravity = true;

    [Header("Lifetime")]
    [SerializeField][Min(0f)] float despawnAfterSeconds = 10f;

    PhysicsMaterial2D runtimeBounceMaterial;

    public override void CastSpell(Transform playerTransform)
    {
        if (essencePrefab == null || playerTransform == null)
        {
            Debug.LogWarning("EssenceSpell: Missing essence prefab or player transform.");
            return;
        }

        EnsureRuntimeMaterial();

        Vector2 center = playerTransform.position;

        for (int i = 0; i < essenceCount; i++)
        {
            float angle = (Mathf.PI * 2f * i) / essenceCount;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 spawnPos = center + (direction * spawnRadius);

            GameObject spawned = Instantiate(essencePrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (zeroGravity)
                {
                    rb.gravityScale = 0f;
                }

                rb.linearDamping = 0f;
                rb.angularDamping = 0f;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.linearVelocity = direction * launchSpeed;
            }

            Collider2D col = spawned.GetComponent<Collider2D>();
            if (col != null)
            {
                col.sharedMaterial = runtimeBounceMaterial;
            }

            Destroy(spawned, despawnAfterSeconds);
        }
    }

    void EnsureRuntimeMaterial()
    {
        if (runtimeBounceMaterial != null)
        {
            return;
        }

        runtimeBounceMaterial = new PhysicsMaterial2D("EssenceSpell_NoFriction_Bounce")
        {
            friction = 0f,
            bounciness = bounciness
        };
    }
}