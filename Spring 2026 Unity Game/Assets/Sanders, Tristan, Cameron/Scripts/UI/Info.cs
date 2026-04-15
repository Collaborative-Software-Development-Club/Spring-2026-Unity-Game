using UnityEngine;

public class Info : MonoBehaviour
{
    // Drag the GameObject you want to toggle into this slot in the Inspector
    public GameObject targetObject;

    // This function will be called by the UI Button
    public void ToggleState()
    {
        if (targetObject != null)
        {
            // Set the state to the opposite of its current local active state
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}