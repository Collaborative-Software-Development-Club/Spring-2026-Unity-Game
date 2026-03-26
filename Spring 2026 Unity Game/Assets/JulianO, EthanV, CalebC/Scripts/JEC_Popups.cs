using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class JEC_Popups : JEC_InteractableBase
{
    public Image adImage;
    [SerializeField] public TMP_Text text;
    public GameObject Player;
    public GameObject popupContent;
    public bool hidingKeys;

    [SerializeField] private int length;
    [SerializeField] private int height;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JEC_Events.OnPopupClosed.AddListener(ClosePopup);

        Player = GameObject.Find("MAIN/Player");
    }

    protected override void Interact()
    {
        JEC_Events.OnPopupClosed.Invoke();
    }

    void ClosePopup()
    {
        Destroy(popupContent);
        Destroy(this.gameObject);
    }

}
