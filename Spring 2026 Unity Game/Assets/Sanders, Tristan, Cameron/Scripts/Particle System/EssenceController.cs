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
    private HashSet<Vector2Int> activeEssenceLocations = new();

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

    private void Awake()
    {
        essenceGrid = new EssenceData[gridWidth, gridHeight];
        InitializeTexture();
    }

    //Advance all active essence by a step
    private void SimulateActiveEssence()
    {
        HashSet<Vector2Int> newActiveLocations = new();
        foreach(var oldLocation in activeEssenceLocations)
        {
            EssenceData essenceAtLocation = essenceGrid[oldLocation.x, oldLocation.y];
            Vector2Int newLocation = oldLocation + essenceAtLocation.velocity;
            
            if(newLocation.x >= 0 && newLocation.y >= 0 && newLocation.x < gridWidth && newLocation.y < gridHeight)
            {
                SetEssence(newLocation.x, newLocation.y, essenceAtLocation);
                newActiveLocations.Add(newLocation);
            }
            
            SetEssence(oldLocation.x,oldLocation.y,EssenceData.Empty);
        }
        activeEssenceLocations = newActiveLocations;
    }

    //Set a point to be a non-moving essence of a specified type
    private void CreateEssence(int posX, int posY, EssenceType type)
    {
        essenceGrid[posX, posY] = new EssenceData { velocity = Vector2Int.zero, type = type};
        essenceTexture.SetPixel(posX, posY, TypeColorDict[type]);
    }

    //Set a point to be a moving essence of a specified type
    private void CreateEssence(int posX, int posY, Vector2Int velocity, EssenceType type)
    {
        essenceGrid[posX, posY] = new EssenceData { velocity = Vector2Int.zero, type = type };

        essenceTexture.SetPixel(posX, posY, TypeColorDict[type]);

        SetVelocity(posX, posY, velocity);
    }

    //Set a point to have the information of a given essence.
    private void SetEssence(int x, int y, EssenceData essenceData)
    {
        essenceGrid[x, y] = essenceData;

        essenceTexture.SetPixel(x, y, TypeColorDict[essenceData.type]);
    }

    //Cause a given point of essence to move.
    private void SetVelocity(int posX, int posY, Vector2Int velocity)
    {
        if (!essenceGrid[posX,posY].Equals(EssenceData.Empty))
        {
            essenceGrid[posX, posY].velocity = velocity;
            if(!activeEssenceLocations.Contains(new Vector2Int(posX, posY)))
            {
                activeEssenceLocations.Add(new Vector2Int(posX, posY));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SimulateActiveEssence();
        essenceTexture.Apply();
    }

    /// <summary>
    /// Creates non-moving essence at the specified point with the specified type
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    public void CreateEssenceWorld(Vector2 position, EssenceType type)
    {
        Vector2Int gridPos = GridUtility.WorldSpaceToGrid(position, gridWidth, gridHeight);
        CreateEssence(gridPos.x,gridPos.y,type);
    }

    /// <summary>
    /// Creates moving essence at the specified point with the specified type
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    public void CreateEssenceWorld(Vector2 position, Vector2Int velocity, EssenceType type)
    {
        Vector2Int gridPos = GridUtility.WorldSpaceToGrid(position, gridWidth, gridHeight);
        CreateEssence(gridPos.x,gridPos.y,velocity,type);
    }

    /// <summary>
    /// Applies a given force to essence at a given position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="force"></param>
    public void ApplyForce(Vector2 position, Vector2Int force)
    {
        Vector2Int gridPos = GridUtility.WorldSpaceToGrid(position, gridWidth, gridHeight);
        Vector2Int oldVelocity = essenceGrid[gridPos.x, gridPos.y].velocity;
        int mass = 1;//May become based off essence data if we give different essence different mass.
        Vector2Int newVelocity =  oldVelocity + (force/mass);
        SetVelocity(gridPos.x, gridPos.y, newVelocity);
    }

}
