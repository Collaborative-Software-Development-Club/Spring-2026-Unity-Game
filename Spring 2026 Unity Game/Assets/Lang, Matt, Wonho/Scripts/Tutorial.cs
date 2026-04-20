using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static bool firstTime = true;
    public ResultDisplayer popups;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (firstTime)
        {
            popups.InfoScreen();
            firstTime = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
