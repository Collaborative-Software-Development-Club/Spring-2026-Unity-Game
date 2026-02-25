using UnityEngine;

public class VineTeleport : MonoBehaviour
{
    public Transform topPoint;
    private bool playerInRange = false;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            Debug.Log("Player near vine");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        if (player != null && topPoint != null)
        {
            player.transform.position = topPoint.position;
            Debug.Log("Teleported!");
        }
    }
}
