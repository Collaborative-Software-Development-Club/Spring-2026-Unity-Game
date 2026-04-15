using UnityEngine;
using System.Collections.Generic;

public class EssenceSpell : SpellBehavior
{
    [Header("Spawn")]
    [SerializeField] GameObject essencePrefab;
    [SerializeField][Min(1)] int essenceCount = 15;
    [SerializeField][Min(0f)] float spawnRadius = 0.35f;
    [SerializeField][Min(0f)] float minSpawnDistanceFromCenter = 0.8f;
    [SerializeField][Min(0f)] float cooldownSeconds = 1f;

    [Header("Motion")]
    [SerializeField][Min(0f)] float launchSpeed = 8f;
    [SerializeField][Min(0f)] float bounciness = 1f;
    [SerializeField] bool zeroGravity = true;

    [Header("Lifetime")]
    [SerializeField][Min(0f)] float despawnAfterSeconds = 10f;

    PhysicsMaterial runtimeBounceMaterial;
    float lastCastTime = float.NegativeInfinity;

    public override void CastSpell(Transform playerTransform)
    {
        if (essencePrefab == null || playerTransform == null)
        {
            Debug.LogWarning("EssenceSpell: Missing essence prefab or player transform.");
            return;
        }

        if (Time.time < lastCastTime + cooldownSeconds)
        {
            return;
        }

        lastCastTime = Time.time;

        EnsureRuntimeMaterial();

        Vector3 center = playerTransform.position;
        float effectiveSpawnRadius = Mathf.Max(spawnRadius, minSpawnDistanceFromCenter);

        for (int i = 0; i < essenceCount; i++)
        {
            float angle = (Mathf.PI * 2f * i) / essenceCount;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Vector3 spawnPos = center + (direction * effectiveSpawnRadius);

            GameObject spawned = Instantiate(essencePrefab, spawnPos, Quaternion.identity);

            Rigidbody rb = spawned.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (zeroGravity)
                {
                    rb.useGravity = false;
                }

                rb.linearDamping = 0f;
                rb.angularDamping = 0f;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.linearVelocity = direction * launchSpeed;
            }

            Collider col = spawned.GetComponent<Collider>();
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

        runtimeBounceMaterial = new PhysicsMaterial("EssenceSpell_NoFriction_Bounce_3D")
        {
            dynamicFriction = 0f,
            staticFriction = 0f,
            bounciness = bounciness,
            frictionCombine = PhysicsMaterialCombine.Minimum,
            bounceCombine = PhysicsMaterialCombine.Maximum
        };
    }
}