using UnityEngine;

public class ShopGUI : MonoBehaviour
{
    public GameObject shop;
    public GameObject shopButton;

    public void ShowShop() {
        shop.SetActive(true);
    }

    public void HideShop() {
        shop.SetActive(false);
    }

    public void ShopAvailable() {
        shopButton.SetActive(true);
    }

    // when the game is not supposed to have the shop available
    public void ShopUnavailable() {
        HideShop();
        shopButton.SetActive(false);
    }
}
