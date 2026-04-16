using Unity.VisualScripting;
using UnityEngine;

public class SolutionChecker : MonoBehaviour
{
    public PlayerControll playerControll;
    public Board board;
    public bool[,] checkBox = new bool[13, 13];
    private int correct = 0;
    public ResultDisplayer result;
    public Stopwatch stopwatch;
    [SerializeField]public BookUnlocker bookUnlocker;
    [SerializeField]public SceneChanger sceneChanger;
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


    public void CheckSolution()
    {
        correct = 0;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                if (checkBox[i, j] != board.gridGenerationScript.visited[j, i] && !(board.gridGenerationScript.startX == j && board.gridGenerationScript.startY == i) && !(board.gridGenerationScript.endX == j && board.gridGenerationScript.endY == i))
                {
                    Debug.Log("INaccuracy at ("+i+", "+j+")");
                    correct++;
                }
            }
        }
        if (correct == 0)
        {
            Debug.Log("You win");
            if (sceneChanger.currentLevel == 1) {
                stopwatch.PauseStopwatch();
            }
            if(sceneChanger.currentLevel == 5)
            {
                Debug.Log("Book should be unlocked now");
                bookUnlocker.Unlock();
            }
            result.WinScreen();
        } else
        {
            Debug.Log("You lose");
            result.LoseScreen();
            if(sceneChanger.currentLevel == 5)
            {
                Debug.Log("Book should be unlocked now");
                bookUnlocker.Unlock();
            }
        }
    }
    
    public void Clear()
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
    
}
