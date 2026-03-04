using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneDamp : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private InputReader inputReader;
    private bool _isPlayerNearby = false;

    private void Awake()
    {
        // Ensure everything is hidden at the start
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }
    private void OnEnable()
    {
        if (inputReader != null) inputReader.InteractEvent += HandleInteraction;
    }

    private void OnDisable()
    {
        if (inputReader != null) inputReader.InteractEvent -= HandleInteraction;
    }

    private void HandleInteraction()
    {
        if (_isPlayerNearby)
        {
            ResetLevel();
        }
    }
    void ResetLevel()
    {
        PlayerInv.water = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerNearby = true;
                if (interactionPrompt != null)
                {
                    interactionPrompt.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerNearby = false;
                if (interactionPrompt != null) interactionPrompt.SetActive(false);
            }
        }
}
