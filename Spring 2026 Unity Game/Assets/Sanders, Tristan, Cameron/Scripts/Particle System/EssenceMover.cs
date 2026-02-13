using UnityEngine;

public class EssenceMover : MonoBehaviour
{
    EssenceController essenceController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        essenceController = GameObject.FindGameObjectWithTag("EssenceSystem").GetComponent<EssenceController>();
    }

    private void Update()
    {
        essenceController.ApplyForce(gameObject.transform.position,Vector2Int.up);
    }

}

