using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class JEC_KeyManager : MonoBehaviour
{
    // Needs to store a list of KeyScriptableObjects
    public List<JEC_Key> Keyboard;

    // Needs to be able to update the list of KeyScriptableObjects

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var key in Keyboard)
        {
            key.amount = 0;
        }
    }

    public JEC_Key FindKey(string character)
    {
        for (int i = 0; i < Keyboard.Count; i++)
        {
            if (Keyboard[i].character == character)
                return Keyboard[i];
        }

        return null;
    }

    public void IncrementKeyVal(string character)
    {
        JEC_Key key = FindKey(character);

        if (key != null)
        {
            key.amount++;
        }
        else
        {
            Debug.LogError("JEC_ERROR: Failed to find key of character: " + character);
        }
    }
}
