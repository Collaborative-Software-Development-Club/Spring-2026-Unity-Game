using UnityEngine;

public class Board : MonoBehaviour
{
    public GridGeneration gridGenerationScript;
    private Row[] rows;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

    private bool test = true;

    private void Update()
    {
        if (test == true)
        {
            Invoke("DrawGrid", 0.5f);
            test = false;
        }
    }

    void DrawGrid()
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                rows[i].tiles[j].SetLetter(gridGenerationScript.grid[i, j]);
            }
        }
    }
}
