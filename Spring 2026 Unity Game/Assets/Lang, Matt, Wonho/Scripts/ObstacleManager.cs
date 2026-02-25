using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public DifficultyManager difficulty;
    private int diff;
    private int randnum;

    public Board highlighter;
    public GridGeneration grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    //creates gray squares
    void CreateRefractionSquares()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDifficultySelected()
    {
        CreateRefractionSquares();
    }
    //create obstacles that can be made after letters are generated
    private void OnGridGeneration()
    {
        BlindBlock();

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
                    highlighter.rows[i].tiles[j].SetColor(Color.darkMagenta);
                    highlighter.rows[i].tiles[j].TextColor(Color.darkMagenta);
                }
            }
        }
    }
}
