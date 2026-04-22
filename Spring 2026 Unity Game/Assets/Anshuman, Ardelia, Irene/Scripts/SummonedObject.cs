using UnityEngine;

public class SummonedObject : MonoBehaviour
{
   public GameObject textBox;
    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            textBox.SetActive(true);
            Invoke("HideText", 2f);
        }
    }

    void HideText()
    {
        textBox.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
