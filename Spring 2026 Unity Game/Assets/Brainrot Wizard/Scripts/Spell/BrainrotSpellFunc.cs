using Unity.VisualScripting;
using UnityEngine;

public class BrainrotSpellFunc : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist > 2.6f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.1f);
        }
        else if (dist < 2.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -0.1f);
        }
    }
}
