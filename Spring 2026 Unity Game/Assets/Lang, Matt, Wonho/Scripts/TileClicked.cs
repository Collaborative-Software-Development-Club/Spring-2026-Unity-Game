using UnityEngine;
using UnityEngine.UI;

public class TileClicked : MonoBehaviour
{
    public int row;
    public int column;
    public PlayerControll control; // reference to main manager

    public void OnClick()
    {
        if (control != null)
        {
            control.TileClicked(row, column);
        }
        else
        {
            Debug.LogWarning("controll not assigned!");
        }
    }
}
