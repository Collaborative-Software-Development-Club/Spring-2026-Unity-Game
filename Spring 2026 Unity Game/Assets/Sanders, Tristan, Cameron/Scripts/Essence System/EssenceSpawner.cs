using System.Collections;
using UnityEngine;

public class EssenceSpawner : MonoBehaviour
{
    [SerializeField] float minXVel;
    [SerializeField] float maxXVel;
    [SerializeField] float minYVel;
    [SerializeField] float maxYVel;
    [SerializeField] GameObject essencePrefab;
    [SerializeField] float spawnRate;
    GameObject essenceContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        essenceContainer = GameObject.FindGameObjectWithTag("EssenceSystem");
        StartCoroutine(ContinuallySpawnEssence());
    }

    private IEnumerator ContinuallySpawnEssence()
    {
        while(true)
        {
            yield return new WaitForSeconds(1 / spawnRate);
            CreateEssence();
        }
    }

    private void CreateEssence()
    {
        var essence = Instantiate(essencePrefab, transform.position, transform.rotation,essenceContainer.transform);
        Vector2 initialVel = new Vector2(Random.Range(minXVel, maxXVel), Random.Range(minYVel, maxYVel));
        essence.GetComponent<Rigidbody>().linearVelocity = CoordConverter.ConvertXYToXZ(initialVel);
            ;
    }

}

