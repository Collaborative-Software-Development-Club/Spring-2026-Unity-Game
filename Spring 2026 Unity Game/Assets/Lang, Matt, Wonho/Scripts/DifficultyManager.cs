using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
public class DifficultyManager : MonoBehaviour
{
    //1 - easy
    //2 - medium
    //3 - hard
    public int difficulty;
    public GridGeneration gridCreator;
    public UnityEvent OnDifficultySelected;
    void Start()
    {
        if (difficulty == 0) difficulty = 1;
        SetDifficulty();
    }
    void SetDifficulty()
    {
        switch (difficulty) {
            case 3:
                gridCreator.angles = AnglesUpToN(6);
                break;
            case 2:
                gridCreator.angles = AnglesUpToN(4);
                break;
            case 1:
                gridCreator.angles = AnglesUpToN(2);
                break;
            default:
                Debug.Log("ERROR: Invalid Difficulty");
                break;
        }
        OnDifficultySelected.Invoke();

    }
    //returns all angles with vector (x,y) where |x+y| <= n and x, y are relatively prime
    //also only adds (1,0),(0,1) and their negative versions if difficulty is easy
    int [,] AnglesUpToN(int n)
    {
        List<List<int>> angles = new List<List<int>>();
        for(int x = 0; x < n; x++)
        {
            for(int y = 0; y <= n-x; y++) {
                if (x!=0 && y!=0 && RelativelyPrime(x, y)) {
                    angles.Add(new List<int> {x, y});
                    angles.Add(new List<int> {-x, y});
                    angles.Add(new List<int> {-x, -y});
                    angles.Add(new List<int> {x, -y});
                }
            }
        }
        if (difficulty == 1)
        {
            angles.Add(new List<int> {1, 0});
            angles.Add(new List<int> {-1,0});
            angles.Add(new List<int> {0,1});
            angles.Add(new List<int> {0,-1});
        }
        int rows = angles.Count;
        int[,] angleSet = new int[rows,2];
        for(int i = 0; i < rows; i++)
        {
            for( int j = 0; j< 2; j++)
            {
                angleSet[i, j] = angles[i][j];
            }
        }
        return angleSet;
    }
    bool RelativelyPrime(int x, int y)
    {
        for(int i = 2; i <= x; i++)
        {
            if (x % i == 0 && y % i == 0)
            {
                return false;
            }
        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
