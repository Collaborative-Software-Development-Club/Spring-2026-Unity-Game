using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Lvl1 levelscript;
    public GameObject[] tutorials;
    public int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorials[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next() {
        tutorials[index].SetActive(false);
        index++;
        if (tutorials.Length <= index) {
            End();
            return;
        }
        tutorials[index].SetActive(true);
    }

    public void Skip() {
        tutorials[index].SetActive(false);
        // handle any missing things here
        End();
    }

    public void End() {

    }
}
