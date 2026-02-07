using UnityEngine;

public class SceneBookUnlocker : MonoBehaviour
{
    [SerializeField] private SpellBookCollection collectionData;
    [SerializeField] private string bookIDForThisScene;
    [SerializeField] private bool unlockOnStart = true;

    private void Start()
    {
        if (unlockOnStart)
        {
            TriggerUnlock();
        }
    }

    public void TriggerUnlock()
    {
        if (collectionData != null)
        {
            collectionData.UnlockBook(bookIDForThisScene);
        }
    }
}