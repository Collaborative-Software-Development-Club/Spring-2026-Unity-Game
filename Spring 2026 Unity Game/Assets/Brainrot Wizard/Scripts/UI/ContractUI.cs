using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractUI : MonoBehaviour
{
    [SerializeField] private Button AcceptButton;
    [SerializeField] private Button DeclineButton;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private GameObject panel;
    
    [SerializeField] private TextMeshProUGUI attributeText;

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
        infoText.text = $"{contract.GetPersonName()}'s contract";

        var turnCount = contract.GetTurnCount();
        
        if(turnCount == 1)
            durationText.text = StringUtils.AbbreviateNumber(contract.GetTurnCount()) + " Turn";
        else
            durationText.text = StringUtils.AbbreviateNumber(contract.GetTurnCount()) + " Turns";

        StringBuilder sb = new StringBuilder();

        void AppendSection(string title, IEnumerable<string> items)
        {
            sb.AppendLine($"<b>{title}</b>"); 
            foreach (var item in items)
            {
                sb.AppendLine("    " + item); 
            }
            sb.AppendLine(); 
        }

        var primaries = contract.GetPrimaryAsString();
        var secondaries = contract.GetSecondaryAsString();
        var optionals = contract.GetOptionalAsString();
        var extras =  contract.GetExtraAsString();

        if (primaries.Count > 0)
            AppendSection("Primary", primaries); 
        if (secondaries.Count > 0)
            AppendSection("Secondary", secondaries);
        if (optionals.Count > 0)
            AppendSection("Optional", optionals);
        if (extras.Count > 0)
            AppendSection("Extra", extras);

        attributeText.text = sb.ToString();
    }

    public void CloseUI()
    {
        panel.SetActive(false);
        _currentContract = null;
        attributeText.text = "";
    }

    private void AcceptCurrentContract()
    {
        if(_currentContract == null) return;
        
        
        GameManager.Instance.ContractManager.TakeContract(_currentContract);
        CloseUI();
    }
    
}
