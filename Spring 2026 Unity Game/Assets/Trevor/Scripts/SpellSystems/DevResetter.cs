// DevResetter.cs
using UnityEngine;

public class DevResetter : MonoBehaviour
{
    [SerializeField] private PlayerProgress progressData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (progressData != null)
            {
                // This clears the save data AND fires the event that updates the bookshelf visuals instantly.
                progressData.ResetProgress();
            }
        }
    }
}