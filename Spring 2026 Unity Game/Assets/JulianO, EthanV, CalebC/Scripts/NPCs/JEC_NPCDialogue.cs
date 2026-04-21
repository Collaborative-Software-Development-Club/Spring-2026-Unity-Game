using UnityEngine;

[CreateAssetMenu(fileName ="NewNPCDialogue", menuName = "NPC Dialogue")]
public class JEC_NPCDialogue: ScriptableObject
{

    public string npcName;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    // may delete for scoping vvv
    public Sprite npcPortrait;

}
