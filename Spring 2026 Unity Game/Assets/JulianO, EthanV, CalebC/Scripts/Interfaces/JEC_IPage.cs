using UnityEngine;

public interface JEC_IPage
{
    Sprite border { get; set; }
    GameObject mainContent { get; set; }
    GameObject bannerAd { get; set; }

    public void UpdateMainContent(GameObject mainContent);

    public void UpdateBannerAd(GameObject bannerAd);
}
