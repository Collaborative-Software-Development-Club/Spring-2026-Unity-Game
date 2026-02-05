using System;
using UnityEngine;

public class BrainrotTest : MonoBehaviour
{
    [SerializeField] private Brainrot brainrot;

    private void Start()
    {
        brainrot.PrintData();
    }
}
