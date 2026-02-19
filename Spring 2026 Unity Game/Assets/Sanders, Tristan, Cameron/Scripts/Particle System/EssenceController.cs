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
    private Essence[,] essenceGrid;
    private HashSet<Vector2Int> activeEssenceLocations = new();

    [SerializeField] int gridHeight;
    [SerializeField] int gridWidth;

    #region Initialization
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
        essenceGrid = new Essence[gridWidth, gridHeight];
        InitializeTexture();
    }
    #endregion

    #region Essence Modification
    //Set a point to be a non-moving essence of a specified type
    private void CreateEssence(int posX, int posY, EssenceType type)
    {
        essenceGrid[posX, posY] = new Essence(Vector2Int.zero, type);
        essenceTexture.SetPixel(posX, posY, TypeColorDict[type]);
    }

    //Set a point to be a moving essence of a specified type
    private void CreateEssence(int posX, int posY, Vector2Int velocity, EssenceType type)
    {
        CreateEssence(posX, posY, type);
        SetVelocity(posX, posY, velocity);
    }

    //Set a point to have the information of a given essence.
    private void SetEssence(int posX, int posY, Essence essence)
    {
        essenceGrid[posX, posY] = essence;

        essenceTexture.SetPixel(posX, posY, TypeColorDict[essence.Type]);
    }

    //Cause a given point of essence to move.
    private void SetVelocity(int posX, int posY, Vector2Int velocity)
    {
        if (!essenceGrid[posX,posY].Equals(Essence.Empty))
        {
            essenceGrid[posX, posY].SetVelocity(velocity);
            if(!activeEssenceLocations.Contains(new Vector2Int(posX, posY)))
            {
                activeEssenceLocations.Add(new Vector2Int(posX, posY));
            }
        }
    }

    #endregion

    #region Essence Simulation
    //Advance all active essence by a step
    private void SimulateActiveEssence()
    {
        HashSet<Vector2Int> newActiveLocations = new();
        foreach (var oldLocation in activeEssenceLocations)
        {
            Essence essenceAtLocation = essenceGrid[oldLocation.x, oldLocation.y];

            Vector2Int newLocation = MoveEssence(essenceAtLocation,oldLocation);
            
            if(essenceAtLocation.GetVelocity().magnitude > 0) newActiveLocations.Add(newLocation);

            SetEssence(oldLocation.x, oldLocation.y, Essence.Empty);
            SetEssence(newLocation.x, newLocation.y, essenceAtLocation);
        }
        activeEssenceLocations = newActiveLocations;
    }

    private Vector2Int MoveEssence(Essence essenceAtLocation, Vector2Int oldLocation)
    {
        return essenceAtLocation.SimulateEssence(oldLocation, gridWidth, gridHeight);
    }

    private Vector2Int CollideEssence(Essence essenceAtLocation, Vector2Int potentialLocation)
    {
        return Vector2Int.zero;
    }

    // Update is called once per frame
    void Update()
    {
        SimulateActiveEssence();
        essenceTexture.Apply();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Creates non-moving essence at the specified point with the specified type
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    public void CreateEssenceWorld(Vector3 position, EssenceType type)
    {
        Vector2Int gridPos = GridUtility.WorldSpaceToGrid(position, gridWidth, gridHeight);
        CreateEssence(gridPos.x,gridPos.y,type);
    }

    /// <summary>
    /// Creates moving essence at the specified point with the specified type
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    public void CreateEssenceWorld(Vector3 position, Vector2Int velocity, EssenceType type)
    {
        Vector2Int gridPos = GridUtility.WorldSpaceToGrid(position, gridWidth, gridHeight);
        CreateEssence(gridPos.x,gridPos.y,velocity,type);
    }

    /// <summary>
    /// Applies a given force to essence at a given position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="force"></param>
    public void ApplyForce(Vector3 position, Vector2Int force)
    {
        Vector2Int gridPos = GridUtility.WorldSpaceToGrid(position, gridWidth, gridHeight);
        if (!GridUtility.IsInBounds(gridPos, gridWidth, gridHeight))
        {
            return;
        }

        if (essenceGrid[gridPos.x, gridPos.y] != null)
        {
            Vector2Int oldVelocity = essenceGrid[gridPos.x, gridPos.y].GetVelocity();
            int mass = 1;//May become based off essence data if we give different essence different mass.
            Vector2Int newVelocity = oldVelocity + (force / mass);
            SetVelocity(gridPos.x, gridPos.y, newVelocity);
        }
    }

    /// <summary>
    /// Applies a given force to all essence in a radius (in grid cells) around a position.
    /// </summary>
    public void ApplyForceArea(Vector3 position, Vector2Int force, int radius, int mass)
    {
        Vector2Int center = GridUtility.WorldSpaceToGrid(position, gridWidth, gridHeight);
        if (!GridUtility.IsInBounds(center, gridWidth, gridHeight))
        {
            return;
        }

        int r = Mathf.Max(0, radius);
        for (int x = center.x - r; x <= center.x + r; x++)
        {
            for (int y = center.y - r; y <= center.y + r; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!GridUtility.IsInBounds(pos, gridWidth, gridHeight))
                {
                    continue;
                }

                Essence essence = essenceGrid[x, y];
                if (essence == null || essence.Equals(Essence.Empty))
                {
                    continue;
                }

                Vector2Int oldVelocity = essence.GetVelocity();
                Vector2Int newVelocity = oldVelocity + (force / mass);
                SetVelocity(x, y, newVelocity);
            }
        }
    }
    #endregion

}
