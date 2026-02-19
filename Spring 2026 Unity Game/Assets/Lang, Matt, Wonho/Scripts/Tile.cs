using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TextMeshProUGUI text;
    public Image image;

    public char Letter { get; private set; }
    public Color Color1 { get; private set; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    public void SetLetter(char letter)
    {
        Letter = letter;
        text.text = letter.ToString();
    }
    
    public void SetColor(Color color)
    {
        Color1 = color;
        image.color = color;
    }
}
