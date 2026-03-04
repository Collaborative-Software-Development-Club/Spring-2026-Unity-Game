using UnityEngine;

public class JEC_TutorialPopup : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private void Start()
    {
        Player.GetComponent<PlayerController>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.GetComponent<PlayerController>().enabled = true;
            gameObject.SetActive(false);
        }

    }
}
