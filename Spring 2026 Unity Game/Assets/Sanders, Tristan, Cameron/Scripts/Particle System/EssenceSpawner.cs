using UnityEngine;

public class EssenceSpawner : MonoBehaviour
{
    EssenceController essenceController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        essenceController = GameObject.FindGameObjectWithTag("EssenceSystem").GetComponent<EssenceController>();
        
    }

    private void Update()
    {
        essenceController.CreateEssenceWorld(gameObject.transform.position, new Vector2Int(-1, 0), EssenceType.normal);
    }

}

