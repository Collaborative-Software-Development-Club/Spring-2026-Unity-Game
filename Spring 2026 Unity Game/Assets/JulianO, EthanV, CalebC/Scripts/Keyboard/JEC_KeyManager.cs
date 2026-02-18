using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class JEC_KeyManager : MonoBehaviour
{
    // Store a list of KeyScriptableObjects
    public List<JEC_Key> Keyboard;

    // Whenever we type in the URL, we need to track how many of each char has been used
    public Dictionary<string, int> KeysUsed;

    void Start()
    {
        JEC_Events.OnPickupKey.AddListener(IncrementKeyVal);

        foreach (var key in Keyboard)
        {
            key.amount = 0;
        }
    }

    public JEC_Key FindKey(string c)
    {
        for (int i = 0; i < Keyboard.Count; i++)
        {
            if (Keyboard[i].character == c)
                return Keyboard[i];
        }

        return null;
    }

    public void ResetKeysTyped()
    {
        KeysUsed = new Dictionary<string, int>();

        foreach(var key in Keyboard)
        {
            KeysUsed[key.character] = 0;
        }
    }

    public void IncrementKeyVal(string c)
    {
        JEC_Key key = FindKey(c);

        if (key != null)
        {
            key.amount++;
        }
        else
        {
            Debug.LogError("JEC_ERROR: Failed to find key of character: " + c);
        }
    }
}
