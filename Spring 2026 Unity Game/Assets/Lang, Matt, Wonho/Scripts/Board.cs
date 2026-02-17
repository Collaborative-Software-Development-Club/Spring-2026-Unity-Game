using UnityEngine;

public class Board : MonoBehaviour
{
    public GridGeneration gridGenerationScript;

    private Row[] rows;

    //private int rowIndex = 0;
    //private int columnIndex = 0;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

    private bool test = true;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            if (test == true)
            {
                //Debug.Log("This is: " + gridGenerationScript.grid[0, 0]);

                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        rows[i].tiles[j].SetLetter(gridGenerationScript.grid[i, j]);
                    }
                }

                test = false;
            }
        }
    }
}
