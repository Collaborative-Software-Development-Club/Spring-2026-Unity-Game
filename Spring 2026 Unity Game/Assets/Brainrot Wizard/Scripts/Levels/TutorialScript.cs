using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameManager gM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gM = GameManager.Instance;
        gM.TutorialStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
