using UnityEngine;

public class EssenceMover : MonoBehaviour
{
    EssenceController essenceController;
    [SerializeField] private Vector2Int force = Vector2Int.up;
    [SerializeField] private int radius = 2;
    [SerializeField] private int mass = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        essenceController = GameObject.FindGameObjectWithTag("EssenceSystem").GetComponent<EssenceController>();
    }

    private void Update()
    {
        if (essenceController == null)
        {
            return;
        }

        if (radius <= 0)
        {
            essenceController.ApplyForce(gameObject.transform.position, force);
            return;
        }

        essenceController.ApplyForceArea(gameObject.transform.position, force, radius, mass);
    }

}

