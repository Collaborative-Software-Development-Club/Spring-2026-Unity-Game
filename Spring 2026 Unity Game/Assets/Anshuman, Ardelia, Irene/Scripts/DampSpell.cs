using UnityEngine;


public class DampSpell : SpellBehavior
{
    public GameObject waterPrefab;     // Assign WaterDrop prefab
    public Transform sprayPoint;       // Optional: where spray starts
    public int amount = 5;             // Number of droplets
    public float speed = 5f;           // Spray speed
    public float spread = 20f;         // Random angle spread
    public float lifeTime = 1f;        // How long droplets stay

    public override void CastSpell(Transform playerTransform)
    {
        // If no spray point assigned, use player position
        Vector3 spawnPos = sprayPoint != null
            ? sprayPoint.position
            : playerTransform.position;

        // Detect facing direction
        float direction = playerTransform.localScale.x >= 0 ? 1f : -1f;

        for (int i = 0; i < amount; i++)
        {
            GameObject drop = Instantiate(waterPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = drop.GetComponent<Rigidbody2D>();

            float angle = Random.Range(-spread, spread);

            Vector2 baseDir = direction > 0 ? Vector2.right : Vector2.left;
            Vector2 finalDir = Quaternion.Euler(0, 0, angle) * baseDir;

            if (rb != null)
                rb.linearVelocity = finalDir * speed;

            Destroy(drop, lifeTime);
        }
    }
}
