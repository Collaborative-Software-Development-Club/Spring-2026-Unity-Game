using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public enum EssenceType { none, normal };

public class EssenceController : MonoBehaviour
{
    private static readonly Dictionary<EssenceType, Color> TypeColorDict = new Dictionary<EssenceType, Color> 
    {   { EssenceType.none, Color.clear },
        { EssenceType.normal, Color.purple } 
    };

    private Texture2D essenceTexture;
    private EssenceData[,] essenceGrid;
    private List<Vector2Int> activeEssenceLocations = new();

    [SerializeField] int gridHeight;
    [SerializeField] int gridWidth;
    //Ensure aspect ratio of grid is aligned with monitor.
    private void OnValidate()
    {
        float aspectRatio = 16f / 9f;
        gridWidth = Mathf.RoundToInt(gridHeight * aspectRatio);
    }

    //Initializes texture of host image to display the essence
    private void InitializeTexture()
    {
        //Create pixel accurate Texture of an apporiate size
        essenceTexture = new Texture2D(gridWidth, gridHeight);
        essenceTexture.filterMode = FilterMode.Point;

        //Make texture clear
        essenceTexture.SetPixels(new Color[gridWidth * gridHeight]);

        //Assign Texture to image component
        Vector2 centerPivot = new Vector2(0.5f, 0.5f);
        GetComponent<Image>().sprite = Sprite.Create(essenceTexture, new Rect(0, 0, gridWidth, gridHeight), centerPivot);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeTexture();
        essenceGrid = new EssenceData[gridWidth, gridHeight];
        SetEssence(25, 25, EssenceType.normal);
    }

    //Advance all active essence by a step
    private void SimulateActiveEssence()
    {
        List<Vector2Int> newActiveLocations = new();
        foreach(var oldLocation in activeEssenceLocations)
        {
            EssenceData essenceAtLocation = essenceGrid[oldLocation.x, oldLocation.y];
            Vector2Int newLocation = oldLocation + essenceAtLocation.velocity;
            
            SetEssence(newLocation.x,newLocation.y,essenceAtLocation);
            newActiveLocations.Add(newLocation);

            SetEssence(oldLocation.x,oldLocation.y,EssenceData.Empty);
        }
        activeEssenceLocations = newActiveLocations;
    }

    //Set a point to be a non-moving essence of a specified type
    private void SetEssence(int x, int y, EssenceType type)
    {
        essenceGrid[x, y] = new EssenceData { velocity = Vector2Int.zero, type = type};
        
        essenceTexture.SetPixel(x, y, TypeColorDict[type]);
    }

    //Set a point to have the information of a given essence.
    private void SetEssence(int x, int y, EssenceData essenceData)
    {
        essenceGrid[x, y] = essenceData;

        essenceTexture.SetPixel(x, y, TypeColorDict[essenceData.type]);
    }

    //Cause a given point of essence to move.
    private void ApplyVelocity(int posX, int posY, Vector2Int velocity)
    {
        if (!essenceGrid[posX,posY].Equals(EssenceData.Empty))
        {
            essenceGrid[posX, posY].velocity = velocity;
            activeEssenceLocations.Add(new Vector2Int(posX, posY));
        }
    }

    // Update is called once per frame
    void Update()
    {
        SimulateActiveEssence();
        essenceTexture.Apply();
    }
}
