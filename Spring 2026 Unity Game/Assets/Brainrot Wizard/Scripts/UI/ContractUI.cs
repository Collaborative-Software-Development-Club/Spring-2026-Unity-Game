using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractUI : MonoBehaviour
{
    [SerializeField] private Button AcceptButton;
    [SerializeField] private Button DeclineButton;
    [SerializeField] private RectTransform InfoPanel;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private GameObject panel;

    [SerializeField] private TextMeshProUGUI textPrefab;

    private Contract _currentContract;

    private void Start()
    {
       DeclineButton.onClick.AddListener(CloseUI); 
       AcceptButton.onClick.AddListener(AcceptCurrentContract);

       GameManager.Instance.onGameStateChange += OpenUIOnContractState;
    }

    private void OpenUIOnContractState(GameState state)
    {
        if (state != GameState.ContractWork) return;

        OpenUI(Contract.GenerateRandomContract());
    }
    
    public void OpenUI(Contract contract)
    {
        panel.SetActive(true);
        _currentContract = contract;
        infoText.text = contract.GetPersonName() + "'s contract";
        
        foreach (var dataString in contract.GetDataAsString())
        {
            TextMeshProUGUI newText = Instantiate(textPrefab, InfoPanel);
            newText.text = dataString;
        }
    }

    public void CloseUI()
    {
        panel.SetActive(false);
        _currentContract = null;

        for (var i = InfoPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(InfoPanel.GetChild(i).gameObject);
        }
    }

    private void AcceptCurrentContract()
    {
        if(_currentContract == null) return;
        
        
        GameManager.Instance.ContractManager.TakeContract(_currentContract);
        CloseUI();
    }
    
}
