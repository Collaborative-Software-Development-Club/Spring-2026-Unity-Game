using UnityEngine;

public class ShopGUI : MonoBehaviour
{
    public GameObject shop;
    public GameObject shopButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
