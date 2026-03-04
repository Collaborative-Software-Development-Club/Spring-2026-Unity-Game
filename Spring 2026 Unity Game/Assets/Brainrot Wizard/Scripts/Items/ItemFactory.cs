using System;
using UnityEngine;

public static class ItemFactory
{
    /// <summary>
    /// Creates a runtime Item from ItemData as a GameObject with a component.
    /// All implemented items (Machine, Brainrot, Lootbox) are MonoBehaviours.
    /// Tool, Seed, and Contract are not implemented yet and throw exceptions.
    /// </summary>
    public static Item CreateItem(ItemData data)
    {
        if (data == null) return null;

        GameObject newObject;

        switch (data.type)
        {
            case ItemType.None:
                Debug.LogWarning("Cannot create an item of type None");
                return null;

            case ItemType.Machine:
                if (data is not MachineData machineData) throw new InvalidCastException("ItemData is not MachineData");

                newObject = new GameObject("Machine");
                Machine machine = newObject.AddComponent<Machine>();
                machine.Initialize(machineData);
                return machine;

            case ItemType.Brainrot:
                if (data is not BrainrotData brainrotData) throw new InvalidCastException("ItemData is not BrainrotData");

                newObject = new GameObject("Brainrot");
                Brainrot brainrot = newObject.AddComponent<Brainrot>();
                brainrot.Initialize(brainrotData);
                return brainrot;

            case ItemType.Lootbox:
                if (data is not Loottable lootboxData) throw new InvalidCastException("ItemData is not Loottable");

                newObject = new GameObject("Lootbox");
                Lootbox lootbox = newObject.AddComponent<Lootbox>();
                lootbox.Initialize(lootboxData);
                return lootbox;

            case ItemType.Tool:
                throw new NotImplementedException("Tool items are not implemented yet.");

            case ItemType.Seed:
                throw new NotImplementedException("Seed items are not implemented yet.");

            case ItemType.Contract:
                throw new NotImplementedException("Contract items are not implemented yet.");

            default:
                throw new ArgumentOutOfRangeException($"Unhandled ItemType: {data.type}");
        }
    }
}