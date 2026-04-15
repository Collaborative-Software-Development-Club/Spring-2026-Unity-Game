using UnityEngine;

public class SummonedObject : MonoBehaviour
{
     public float spawnDistance = 2f;
    public GameObject textBox;

    private Transform player;
    private bool playerNearby = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Spawn in front of player
        transform.position = player.position + player.forward * spawnDistance;
        transform.rotation = player.rotation;

        textBox.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            textBox.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            textBox.SetActive(false);
        }
    }
}
