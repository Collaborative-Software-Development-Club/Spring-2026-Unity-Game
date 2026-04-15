using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting; // Required for IPointerExitHandler etc. if needed

public class PlayerControll : MonoBehaviour
{
    [SerializeField] public RectTransform CanvasRectTransform; // Reference to the Canvas's RectTransform
    [SerializeField] public Camera UICamera; // The camera rendering the UI (optional, see notes below)
    public Rect screen;
    public Board board;
    public int gridx = -100;
    public int gridy = -100;
    public SolutionChecker state;
    private float gridsize;

    private void Start()
    {
        gridx = -100;
        gridy = -100;
        screen = CanvasRectTransform.rect;
        gridsize = screen.width / 13;
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            FindLoc();
            SelectedSolution();
            Debug.Log("The current grid location is " + gridx + ", " + gridy);
        }*/

    }
    public void TileClicked(int row, int col)
    {
        Debug.Log("Tile " + row + ", " + col+ "clicked.");
        gridy = row;
        gridx = col;
        SelectedSolution();
    }
/*
    void FindLoc()
    {
        screen = CanvasRectTransform.rect;
        gridsize = screen.width / 13;
        // Get the current mouse position in screen coordinates
        Vector2 screenPoint = Input.mousePosition;

        Vector2 localPoint;

        // Convert the screen point to a local point within the CanvasRectTransform's rectangle
        bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            CanvasRectTransform,
            screenPoint,
            UICamera, // Pass null if Canvas Render Mode is 'Screen Space - Overlay'
            out localPoint
        );


        if (localPoint.x < screen.xMax && localPoint.x > screen.xMin && localPoint.y < screen.yMax && localPoint.y > screen.yMin)
        {
            tempx = (localPoint.x - screen.xMin) / gridsize;
            tempy = (-localPoint.y + screen.yMax) / gridsize;
        }
        else
        {
            tempx = -100;
            tempy = -100;
        }

        gridx = (int)Mathf.Floor(tempx);
        gridy = (int)Mathf.Floor(tempy);
    }*/
    public void SelectedSolution()
    {

        if (!state.checkBox[gridx, gridy])
        {
            if (!(board.gridGenerationScript.startX == gridy && board.gridGenerationScript.startY == gridx) && !(board.gridGenerationScript.endX == gridy && board.gridGenerationScript.endY == gridx)) {
                board.rows[gridy].tiles[gridx].SetColor(Color.gold);
                state.checkBox[gridx, gridy] = true;
            }
        }
        else
        {
            
            if (board.gridGenerationScript.startX == gridy && board.gridGenerationScript.startY == gridx)
            {
                board.rows[gridy].tiles[gridx].SetColor(Color.red);
            }
            else if (board.gridGenerationScript.endX == gridy && board.gridGenerationScript.endY == gridx)
            {
                board.rows[gridy].tiles[gridx].SetColor(Color.green);
            }
            else
            {
                if (board.obstacles.isBlind[gridy,gridx]) 
                {
                    board.rows[gridy].tiles[gridx].SetColor(Color.darkMagenta);
                }
                else /*if (!(board.gridGenerationScript.startX == gridy && board.gridGenerationScript.startY == gridx) && !(board.gridGenerationScript.endX == gridy && board.gridGenerationScript.endY == gridx))*/
                {
                    board.rows[gridy].tiles[gridx].SetColor(Color.black);

                }

            }
            state.checkBox[gridx, gridy] = false;
        }
    }
}
