using UnityEngine;

public class ProgressStage : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Submit Progress"; 
    public bool Interact(Interacter interactor)
    {
        StartCoroutine(
            NotificationUI.Instance.RequestConfirmation(
                "Submit Progress",
                "Finish the task for the day and move on to the next task?",
                result =>
                {
                    if (result)
                    {
                        GameManager.Instance.NextState();
                    }
                }
            )
        );

        return true;
    }
}
