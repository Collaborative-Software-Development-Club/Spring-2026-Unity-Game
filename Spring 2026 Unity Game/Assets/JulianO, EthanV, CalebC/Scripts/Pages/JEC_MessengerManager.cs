using System.Collections.Generic;
using UnityEngine;

public class JEC_MessengerManager : MonoBehaviour
{
    public static JEC_MessengerManager Instance { get; private set; }

    [SerializeField] private List<JEC_MessengerMessage> startingMessages = new List<JEC_MessengerMessage>();
    [SerializeField] private List<JEC_PageMessengerTrigger> pageMessages = new List<JEC_PageMessengerTrigger>();

    private readonly List<JEC_MessengerMessage> receivedMessages = new List<JEC_MessengerMessage>();
    private readonly HashSet<JEC_PageMessengerTrigger> firedPageMessageTriggers = new HashSet<JEC_PageMessengerTrigger>();

    public IReadOnlyList<JEC_MessengerMessage> ReceivedMessages => receivedMessages;
    public bool HasUnreadMessages { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        JEC_Events.OnPageChanged.AddListener(HandlePageChanged);

        foreach (JEC_MessengerMessage message in startingMessages)
        {
            ReceiveMessage(message, false);
        }
    }

    private void OnDestroy()
    {
        JEC_Events.OnPageChanged.RemoveListener(HandlePageChanged);

        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void ReceiveMessage(string senderName, string messageBody, bool autoOpenOnReceive = false)
    {
        JEC_MessengerMessage message = new JEC_MessengerMessage
        {
            senderName = senderName,
            messageBody = messageBody,
            autoOpenOnReceive = autoOpenOnReceive
        };

        ReceiveMessage(message, true);
    }

    public void MarkMessagesRead()
    {
        SetUnreadState(false);
    }

    private void HandlePageChanged(JEC_PageData pageData)
    {
        if (pageData == null)
        {
            return;
        }

        foreach (JEC_PageMessengerTrigger trigger in pageMessages)
        {
            if (trigger == null || trigger.page != pageData || trigger.message == null)
            {
                continue;
            }

            if (trigger.sendOnlyOnce && firedPageMessageTriggers.Contains(trigger))
            {
                continue;
            }

            ReceiveMessage(trigger.message, true);

            if (trigger.sendOnlyOnce)
            {
                firedPageMessageTriggers.Add(trigger);
            }
        }
    }

    private void ReceiveMessage(JEC_MessengerMessage message, bool dispatchAutoOpen)
    {
        if (message == null)
        {
            return;
        }

        receivedMessages.Add(message);
        SetUnreadState(true);
        JEC_Events.OnMessengerMessageReceived.Invoke(message);

        if (dispatchAutoOpen && message.autoOpenOnReceive)
        {
            JEC_Events.OnInteractMessengerPedestal.Invoke();
        }
    }

    private void SetUnreadState(bool hasUnreadMessages)
    {
        if (HasUnreadMessages == hasUnreadMessages)
        {
            return;
        }

        HasUnreadMessages = hasUnreadMessages;
        JEC_Events.OnMessengerUnreadChanged.Invoke(HasUnreadMessages);
    }
}
