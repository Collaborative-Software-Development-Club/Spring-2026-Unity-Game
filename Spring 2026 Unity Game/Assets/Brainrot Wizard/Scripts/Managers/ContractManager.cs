using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    private List<Contract> _contracts = new();
    [SerializeField] private ContractDatabase contractDatabase;
    private void Start()
    {
        GameManager.Instance.onTurnChange += OnTurnChange;
    }

    public void AddContract(Contract contract)
    {
        _contracts.Add(contract);
    }
    

    public bool RemoveContract(Contract contractToRemove)
    {
        return Enumerable.Contains(_contracts, contractToRemove) && _contracts.Remove(contractToRemove);
    }

    public bool HasContract(Contract contractToCheck)
    {
       return  _contracts.Contains(contractToCheck); 
    }

    public bool TurnInContract(Contract contractToTurnIn)
    {
        if (!HasContract(contractToTurnIn)) return false;

        double contractValue = contractToTurnIn.GetValue();
        GameManager.Instance.EconomyManager.AddMoney(contractValue);
        return _contracts.Remove(contractToTurnIn);
    }

    private void OnTurnChange(int currentTurn)
    {
        foreach (var contract in _contracts)
        {
            contract.DecrementDuration();
            
            if(contract.IsPastDuration())
                RemoveContract(contract);
        } 
    }

    public List<Contract> GetContracts()
    {
        return  _contracts;
    }

    public ContractDatabase GetContractDatabase()
    {
        return contractDatabase;
    }
}
