using UnityEngine;

public class Board : MonoBehaviour
{
    public GridGeneration gridGenerationScript;
    private Row[] rows;

    private int randnum;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

   // private bool test = true;

    private void OnGridGeneration()
    {
        //if (test == true)
      //  {
            DrawGrid();
            ShowSolution();
            BlindBlock();
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

    void BlindBlock()
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                randnum = Random.Range(0, 100);
                if(randnum < 10)
                {
                    rows[i].tiles[j].SetColor(Color.darkMagenta);
                    rows[i].tiles[j].TextColor(Color.darkMagenta);
                }
            }
        }
    }
}
