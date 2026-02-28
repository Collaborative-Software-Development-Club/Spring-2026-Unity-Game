using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting; // Required for IPointerExitHandler etc. if needed

public class PlayerControll : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform; // Reference to the Canvas's RectTransform
    [SerializeField] private Camera uiCamera; // The camera rendering the UI (optional, see notes below)

    private float tempx = -100;
    private float tempy = -100;

    public int gridx = -100;
    public int gridy = -100;

    private float gridsize = 1000 / 13;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindLoc();
            Debug.Log("The current grid location is " + gridx + ", " + gridy);
        }

    }

    void FindLoc()
    {
        // Get the current mouse position in screen coordinates
        Vector2 screenPoint = Input.mousePosition;

        Vector2 localPoint;

        // Convert the screen point to a local point within the canvasRectTransform's rectangle
        bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPoint,
            uiCamera, // Pass null if Canvas Render Mode is 'Screen Space - Overlay'
            out localPoint
        );

        if (localPoint.x < 500 && localPoint.x > -500 && localPoint.y < 500 && localPoint.y > -500)
        {
            tempx = (localPoint.x + 500) / gridsize;
            tempy = (-localPoint.y + 500) / gridsize;
        }
        else
        {
            tempx = -100;
            tempy = -100;
        }

        gridx = (int)Mathf.Ceil(tempx);
        gridy = (int)Mathf.Ceil(tempy);
    }
}
