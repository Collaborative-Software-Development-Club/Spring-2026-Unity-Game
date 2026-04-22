using UnityEngine;


public class DampSpell : SpellBehavior
{
    public GameObject waterPrefab;     // Assign WaterDrop prefab
    public Transform sprayPoint;       // Optional: where spray starts
    public int amount = 5;             // Number of droplets
    public float speed = 5f;           // Spray speed
    public float spread = 20f;         // Random angle spread
    public float lifeTime = 1f;        // How long droplets stay

    public float spawnRadius = 1f;

    public override void CastSpell(Transform playerTransform)
    {
        // If no spray point assigned, use player position
        Vector3 centerPos = sprayPoint != null
            ? sprayPoint.position
            : playerTransform.position;

        // Detect facing direction
        float direction = playerTransform.localScale.x >= 0 ? 1f : -1f;

        for (int i = 0; i < amount; i++)
        {
            float angle = i * (360f / amount);
            Vector2 dir = new Vector2(
        Mathf.Cos(angle * Mathf.Deg2Rad),
        Mathf.Sin(angle * Mathf.Deg2Rad)
    );
            Vector3 spawnPos = centerPos + (Vector3)(dir * spawnRadius);

            GameObject drop = Instantiate(
                waterPrefab,
                spawnPos,
                Quaternion.Euler(90f, 0f, 0f)
            );
            Rigidbody2D rb = drop.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir * speed;
            Destroy(drop, lifeTime);
        }
    }
}
