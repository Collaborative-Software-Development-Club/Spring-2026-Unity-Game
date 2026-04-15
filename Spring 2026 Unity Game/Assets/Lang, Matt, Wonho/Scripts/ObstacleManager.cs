using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public SceneChanger level;
    public DifficultyManager difficulty;
    private int diff;
    private int randnum;

    public Board highlighter;
    public GridGeneration grid;
    public bool[,] isBlind = new bool[13,13];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        diff = difficulty.difficulty;   
        isBlind = new bool[13,13];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDifficultySelected()
    {
        
    }
    //create obstacles that can be made after letters are generated
    public void OnGridGeneration()
    {
        BlindBlock();

    }
    void BlindBlock()
    {
        if (level.currentLevel == 3 || level.currentLevel == 5) {
            Color blindColor = new Color(1f,1f,1f,0f);
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (!(grid.startX == j && grid.startY == i) && !(grid.endX == j && grid.endY == i))
                    {
                        randnum = Random.Range(0, 100);
                        if(randnum < 10)
                        {
                            highlighter.rows[i].tiles[j].SetColor(Color.darkMagenta);
                            highlighter.rows[i].tiles[j].TextColor(blindColor);
                            isBlind[i,j] = true;
                        }
                    }
                }
            }
            highlighter.HighlightGrid();
        }
    }
}
