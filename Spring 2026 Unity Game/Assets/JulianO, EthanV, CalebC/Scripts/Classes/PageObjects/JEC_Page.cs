using UnityEngine;

public class JEC_Page : MonoBehaviour
{
    private GameObject mainContent;
    private GameObject bannerAd;

    public void UpdateMainContent(GameObject mainContent)
    {
        this.mainContent = mainContent;
    }
    public void UpdateBannerAd(GameObject bannerAd)
    {
        this.bannerAd = bannerAd;
    }
}
