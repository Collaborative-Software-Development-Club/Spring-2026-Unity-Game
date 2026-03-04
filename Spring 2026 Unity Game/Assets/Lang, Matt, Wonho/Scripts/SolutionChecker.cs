using UnityEngine;

public class SolutionChecker : MonoBehaviour
{
    public PlayerControll playerControll;
    public Board board;
    public bool[,] checkBox = new bool[13, 13];
    private int correct = 0;
    public ResultDisplayer result;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                checkBox[i, j] = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckSolution();
        }
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        */
    }


    void CheckSolution()
    {
        correct = 0;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                if (checkBox[i, j] != board.gridGenerationScript.visited[j, i])
                {
                    Debug.Log("INaccuracy at ("+i+", "+j+")");
                    correct++;
                }
            }
        }
        if (correct == 0)
        {
            Debug.Log("You win");
            result.WinScreen();
        } else
        {
            Debug.Log("You lose");
            result.LoseScreen();
        }
    }
    /*
    void Reset()
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                checkBox[j, i] = true;
                playerControll.gridx = j;
                playerControll.gridy = i;
                playerControll.SelectedSolution();
                checkBox[j,i] = false;
            }
        }
    }
    */
}
