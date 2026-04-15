using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class JEC_MessengerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text historyText;

    private void Start()
    {
        JEC_Events.OnMessengerMessageReceived.AddListener(HandleMessageReceived);
        JEC_Events.OnInteractMessengerPedestal.AddListener(HandleMessengerOpened);

        RefreshMessageDisplay();
    }

    private void OnDestroy()
    {
        JEC_Events.OnMessengerMessageReceived.RemoveListener(HandleMessageReceived);
        JEC_Events.OnInteractMessengerPedestal.RemoveListener(HandleMessengerOpened);
    }

    private void OnEnable()
    {
        RefreshMessageDisplay();

        if (JEC_MessengerManager.Instance != null)
        {
            JEC_MessengerManager.Instance.MarkMessagesRead();
        }
    }

    public void CloseMessenger()
    {
        JEC_Events.OnExitMessengerPedestal.Invoke();
    }

    private void HandleMessageReceived(JEC_MessengerMessage message)
    {
        RefreshMessageDisplay();
    }

    private void HandleMessengerOpened()
    {
        RefreshMessageDisplay();

        if (JEC_MessengerManager.Instance != null)
        {
            JEC_MessengerManager.Instance.MarkMessagesRead();
        }
    }

    private void RefreshMessageDisplay()
    {
        IReadOnlyList<JEC_MessengerMessage> messages = JEC_MessengerManager.Instance != null
            ? JEC_MessengerManager.Instance.ReceivedMessages
            : null;

        if (messages == null || messages.Count == 0)
        {
            if (senderText != null)
            {
                senderText.SetText("No Messages");
            }

            if (messageText != null)
            {
                messageText.SetText("No one has reached out yet.");
            }

            if (historyText != null)
            {
                historyText.SetText(string.Empty);
            }

            return;
        }

        JEC_MessengerMessage latestMessage = messages[messages.Count - 1];

        if (senderText != null)
        {
            senderText.SetText(latestMessage.senderName);
        }

        if (messageText != null)
        {
            messageText.SetText(latestMessage.messageBody);
        }

        if (historyText != null)
        {
            StringBuilder historyBuilder = new StringBuilder();

            for (int i = messages.Count - 1; i >= 0; i--)
            {
                JEC_MessengerMessage message = messages[i];
                historyBuilder.Append(message.senderName);
                historyBuilder.Append(": ");
                historyBuilder.Append(message.messageBody);

                if (i > 0)
                {
                    historyBuilder.AppendLine();
                    historyBuilder.AppendLine();
                }
            }

            historyText.SetText(historyBuilder.ToString());
        }
    }

}
