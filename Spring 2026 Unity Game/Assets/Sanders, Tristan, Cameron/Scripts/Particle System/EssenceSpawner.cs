using UnityEngine;

public class EssenceSpawner : MonoBehaviour
{
    [SerializeField] Vector2Int initialVel;
    EssenceController essenceController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        essenceController = GameObject.FindGameObjectWithTag("EssenceSystem").GetComponent<EssenceController>();
        
    }

    private void Update()
    {
        essenceController.CreateEssenceWorld(gameObject.transform.position, initialVel, EssenceType.normal);
    }

}

