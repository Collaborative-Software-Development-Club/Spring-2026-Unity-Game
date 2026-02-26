using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class JEC_NPC : JEC_InteractableBase
{
    public JEC_NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public GameObject Player;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private object npcPortrait;

    private void Start()
    {
        JEC_Events.OnStartNPC.AddListener(StartDialogue);
        JEC_Events.ProgressDialogueNPC.AddListener(NextLine);
        JEC_Events.LeaveNPC.AddListener(EndDialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            JEC_Events.LeaveNPC.Invoke();
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    protected override void Interact()
    {
        if (dialogueData == null) 
        {
            return;
        }
        if (isDialogueActive)
        {
            JEC_Events.ProgressDialogueNPC.Invoke();
        } 
        else
        {
            JEC_Events.OnStartNPC.Invoke();
        }
    }

    void StartDialogue() 
    { 
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.name);
        // if doing portraits insert here

        dialoguePanel.SetActive(true);
        Player.GetComponent<PlayerController>().enabled = false;
        Player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            // if NPC is already typing, autocompletes the line
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        // if there is another line remaining, start the next line
        else if (++dialogueIndex < (dialogueData.dialogueLines.Length))
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            // fill in the text box with the dialogue one char at a time
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        Player.GetComponent<PlayerController>().enabled = true;

    }

}
