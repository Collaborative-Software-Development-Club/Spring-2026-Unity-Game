using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellData", menuName = "Systems/Spell Data")]
public class SpellData : ScriptableObject
{
    public LevelID spellID; // Matches your existing enum
    public string spellName;
    public Sprite spellIcon;

    [TextArea(3, 5)]
    public string loreDescription;

    [TextArea(3, 5)]
    public string mechanicDescription;

    [Tooltip("The prefab containing the specific SpellBehavior script created by the team.")]
    public SpellBehavior spellPrefab;
}