using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class JEC_NPC : MonoBehaviour
{
    public JEC_NPC dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
}
