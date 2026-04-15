using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TerminalItemUI : MonoBehaviour
{
    private Contract _contract;
    
    [SerializeField] private Button turnInButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI contractNameText;
    [SerializeField] private TextMeshProUGUI contractValueText;
    [SerializeField] private TextMeshProUGUI turnsLeftText;
    [SerializeField] private TextMeshProUGUI attributeText;
    

    private void Start()
    {
        turnInButton.onClick.AddListener(OnTurnIn);
        cancelButton.onClick.AddListener(OnCancelContract);
    }

    public void SetContract(Contract contract)
    {
        _contract = contract;

        contractNameText.text = contract.GetPersonName();

        var turnCount = contract.GetTurnCount();
        
        if(turnCount == 1)
            turnsLeftText.text = StringUtils.AbbreviateNumber(contract.GetTurnCount()) + " Turn";
        else
            turnsLeftText.text = StringUtils.AbbreviateNumber(contract.GetTurnCount()) + " Turns";

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

    private void OnTurnIn()
    {
        GameManager.Instance.ContractManager.TurnInContract(_contract);
    }

    private void OnCancelContract()
    {
        if (GameManager.Instance.ContractManager.RemoveContract(_contract))
        {
            Destroy(gameObject);
        }
    }
}
