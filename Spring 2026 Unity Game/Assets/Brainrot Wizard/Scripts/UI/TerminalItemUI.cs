using System;
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
    

    private void Start()
    {
        turnInButton.onClick.AddListener(OnTurnIn);
        cancelButton.onClick.AddListener(OnCancelContract);
    }

    public void SetContract(Contract contract)
    {
        _contract = contract;

        contractNameText.text = contract.GetPersonName();
        //contractValueText.text = StringUtils.AbbreviateNumber(contract.GetValue());
        turnsLeftText.text = StringUtils.AbbreviateNumber(contract.GetTurnCount());
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
