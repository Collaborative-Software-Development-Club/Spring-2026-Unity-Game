using UnityEngine;

public class Board : MonoBehaviour
{
    public GridGeneration gridGenerationScript;
    public PlayerControll playerControll;
    public Row[] rows;

    public bool show = false;


    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

    private void Update()
    {
        selectedSolution();
    }

    // private bool test = true;

    private void OnGridGeneration()
    {
        //if (test == true)
      //  {
        DrawGrid();
        if (show)
        {
            ShowSolution();
        }
        HighlightGrid();
        //test = false;
        //   }
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

    void HighlightGrid()
    {
        rows[gridGenerationScript.startX].tiles[gridGenerationScript.startY].SetColor(Color.red);
        rows[gridGenerationScript.startX].tiles[gridGenerationScript.startY].TextColor(Color.white);
        rows[gridGenerationScript.endX].tiles[gridGenerationScript.endY].SetColor(Color.green);
        rows[gridGenerationScript.endX].tiles[gridGenerationScript.endY].TextColor(Color.white);
    }
    //Since we are too dumb to solve our own puzzle
    void ShowSolution()
    {
        for(int i = 0; i < 13; i++)
        {
            for(int j = 0; j < 13; j++)
            {
                if (gridGenerationScript.visited[i,j])
                {
                    rows[i].tiles[j].SetColor(Color.blue);
                }
            }
        }
    }

    void selectedSolution()
    {
        if (playerControll.gridx * playerControll.gridy != 10000)
        {
            rows[playerControll.gridy].tiles[playerControll.gridx].SetColor(Color.gold);
        }
    }

}
