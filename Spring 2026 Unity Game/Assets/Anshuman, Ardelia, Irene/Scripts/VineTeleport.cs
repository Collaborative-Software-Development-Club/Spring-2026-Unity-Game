using UnityEngine;

public class VineTeleport : MonoBehaviour
{
    public Transform topPoint;
    private bool playerInRange = false;
    private GameObject player;
    [SerializeField] private GameObject popupPrompt;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupPrompt.SetActive(true);
            playerInRange = true;
            player = other.gameObject;
            Debug.Log("Player near vine");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupPrompt.SetActive(false);
            playerInRange = false;
            player = null;
        }
    }

    private void Awake()
    {
        popupPrompt.SetActive(false);
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
