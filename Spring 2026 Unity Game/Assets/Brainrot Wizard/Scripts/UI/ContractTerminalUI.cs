using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractTerminalUI : MonoBehaviour
{
    [SerializeField] private GameObject contractPanel;
    [SerializeField] private Transform contentArea;
    [SerializeField] private Button closeButton;

    [SerializeField] private GameObject terminalItemPrefab;

    private void Start()
    {
        closeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        var contracts = GameManager.Instance.ContractManager.GetContracts();

        foreach (Transform content in contentArea)
        {
            Destroy(content.gameObject);
        }

        foreach (Contract contract in contracts)
        {
            GameObject terminalItem = Instantiate(terminalItemPrefab, contentArea);
            terminalItem.GetComponent<TerminalItemUI>().SetContract(contract);
        }
        contractPanel.SetActive(true);
    }

    public void Hide()
    {
       contractPanel.SetActive(false); 
    }

    public bool IsVisible()
    {
        return contractPanel.activeSelf;
    }
}
