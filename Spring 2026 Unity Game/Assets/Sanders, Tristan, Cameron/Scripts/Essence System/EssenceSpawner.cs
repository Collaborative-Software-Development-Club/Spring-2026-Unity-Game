using System.Collections;
using UnityEngine;

public class EssenceSpawner : MonoBehaviour
{
    [SerializeField] Vector2 spawnDirection;
    [SerializeField][Tooltip("The max amount which the spawnDirection can vary from the default value")] float spawnVariance;
    [SerializeField] GameObject essencePrefab;
    [SerializeField][Tooltip("How many are spawned per second")] float spawnRate;
    GameObject essenceContainer;

    int spawnCount = 0;
    [SerializeField][Tooltip("Max amount of essence allowed to be spawned")] int spawnMaximum = 5000;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        essenceContainer = GameObject.FindGameObjectWithTag("EssenceSystem");
        StartCoroutine(ContinuallySpawnEssence());
    }

    private IEnumerator ContinuallySpawnEssence()
    {
        while(spawnCount < spawnMaximum)
        {
            yield return new WaitForSeconds(1 / spawnRate);
            CreateEssence();
        }
    }

    private void CreateEssence()
    {
        var essence = Instantiate(essencePrefab, transform.position, transform.rotation,essenceContainer.transform);
        Vector2 initialVel = GenerateSpawnVector();
        essence.GetComponent<Rigidbody2D>().linearVelocity = initialVel;
        spawnCount++;
    }

    private Vector2 GenerateSpawnVector()
    {
        float xVariance = Random.Range(-spawnVariance, spawnVariance);
        float yVariance = Random.Range(-spawnVariance, spawnVariance);
        return new Vector2(spawnDirection.x + xVariance, spawnDirection.y + yVariance);
    }

}

