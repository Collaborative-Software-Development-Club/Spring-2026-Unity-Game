using UnityEngine;
using UnityEngine.UI;

public class EssenceController : MonoBehaviour
{
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;
    private Texture2D essenceTexture;
    private int[,] essenceGrid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        essenceGrid = new int[gridWidth, gridHeight];
        essenceTexture = new Texture2D(gridWidth, gridHeight);
        essenceTexture.filterMode = FilterMode.Point;
        GetComponent<Image>().sprite = Sprite.Create(essenceTexture, new Rect(0,0,gridWidth,gridHeight),new Vector2(0.5f,0.5f),100f);
        SetPixel(250, 250, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Obtains the texture storing the location of all essence on a grid.
    /// </summary>
    /// <returns></returns>
    public Texture2D GetEssenceTexture()
    {
        return essenceTexture;
    }

    private void SetPixel(int x, int y, int colorVal)
    {
        essenceGrid[x, y] = colorVal;
        essenceTexture.SetPixel(x, y, Color.black);
        essenceTexture.Apply();
    }
}
