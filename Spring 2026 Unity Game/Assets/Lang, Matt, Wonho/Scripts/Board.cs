using UnityEngine;

public class Board : MonoBehaviour
{
    public GridGeneration gridGenerationScript;
    public PlayerControll playerControll;
    public Row[] rows;

    private bool[,] checkBox = new bool[13, 13];
    private bool checker;
    private int correct = 0;

    public bool show = false;


    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                checkBox[i, j] = false;
            }
        }
    }

    private void Update()
    {
        SelectedSolution();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckSolution();
        }
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

    void SelectedSolution()
    {
        if (Input.GetMouseButtonDown(0))
        {
            checker = true;
        }

        if (playerControll.gridx * playerControll.gridy != 10000 && checkBox[playerControll.gridx, playerControll.gridy] == false && checker == true)
        {
            rows[playerControll.gridy].tiles[playerControll.gridx].SetColor(Color.gold);
            checkBox[playerControll.gridx, playerControll.gridy] = true;
            checker = false;
        }

        if (playerControll.gridx * playerControll.gridy != 10000 && checkBox[playerControll.gridx, playerControll.gridy] == true && checker == true)
        {
            rows[playerControll.gridy].tiles[playerControll.gridx].SetColor(Color.black);
            checkBox[playerControll.gridx, playerControll.gridy] = false;
            checker = false;
        }
    }

    void CheckSolution()
    {
        correct = 0;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                if (checkBox[i, j] != gridGenerationScript.visited[j, i])
                {
                    correct++;
                }
            }
        }
        if (correct == 0)
        {
            Debug.Log("You win");
        } else
        {
            Debug.Log("You lose");
        }
    }
}
