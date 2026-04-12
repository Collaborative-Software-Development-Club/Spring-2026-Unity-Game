using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    private List<Contract> _contracts = new();
    [SerializeField] private ContractDatabase contractDatabase;
    
    public Action onContractTaken;
    public Action<Contract> onContractFailed;
    public Action<Contract> onContractTurnedIn;
    
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

    public void TakeContract(Contract contract)
    {
        onContractTaken?.Invoke();
        AddContract(contract);
    }
    public bool TurnInContract(Contract contractToTurnIn)
    {
        if (!HasContract(contractToTurnIn)) return false;

        Debug.LogWarning("No way to calculate contract worth yet!");
        //double contractValue = contractToTurnIn.GetValue();
        //GameManager.Instance.EconomyManager.AddMoney(contractValue);
        onContractTurnedIn?.Invoke(contractToTurnIn);
        return _contracts.Remove(contractToTurnIn);
    }

    private void OnTurnChange(int currentTurn)
    {
        foreach (Contract contract in _contracts)
        {
            contract.DecrementDuration();

            if (!contract.IsPastDuration()) continue;
            
            onContractFailed?.Invoke(contract);
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
