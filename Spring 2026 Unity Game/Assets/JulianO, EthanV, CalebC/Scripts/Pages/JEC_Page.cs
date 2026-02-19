using UnityEngine;

public class JEC_Page : MonoBehaviour
{
    [Header("Universal Elements")]
    [SerializeField] private GameObject border;
    //[SerializeField] private GameObject searchBar;
    //[SerializeField] private GameObject keyboard;

    [Header("Layout Slots")]
    [SerializeField] private Transform mainContentSlot;
    [SerializeField] private Transform bannerAdSlot;

    private GameObject currentMainContent;
    private GameObject currentBannerAd;

    private void Awake()
    {
    }

    public void SetMainContent(GameObject contentPrefab)
    {
        ReplaceInSlot(ref currentMainContent, contentPrefab, mainContentSlot);
    }

    public void SetBannerAd(GameObject bannerPrefab)
    {
        ReplaceInSlot(ref currentBannerAd, bannerPrefab, bannerAdSlot);
    }
    private void ReplaceInSlot(ref GameObject current, GameObject prefab, Transform slot)
    {
        if (current != null)
            Destroy(current);

        current = Instantiate(prefab, slot);
        current.transform.localPosition = Vector3.zero;
        current.transform.localRotation = Quaternion.identity;
        current.transform.localScale = Vector3.one;
    }
}
