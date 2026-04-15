using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JEC_MessengerPedestal : JEC_InteractableBase
{
    [SerializeField] private GameObject messengerUI;
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer unreadIndicator;
    [SerializeField] private Color unreadColor = Color.red;
    [SerializeField] private Color readColor = Color.white;

    private bool isActive;

    private void Start()
    {
        JEC_Events.OnInteractMessengerPedestal.AddListener(EnterMessenger);
        JEC_Events.OnExitMessengerPedestal.AddListener(ExitMessenger);
        JEC_Events.OnMessengerUnreadChanged.AddListener(UpdateUnreadIndicator);

        if (messengerUI != null)
        {
            messengerUI.SetActive(false);
        }

        bool hasUnreadMessages = JEC_MessengerManager.Instance != null && JEC_MessengerManager.Instance.HasUnreadMessages;
        UpdateUnreadIndicator(hasUnreadMessages);
    }

    private void OnDestroy()
    {
        JEC_Events.OnInteractMessengerPedestal.RemoveListener(EnterMessenger);
        JEC_Events.OnExitMessengerPedestal.RemoveListener(ExitMessenger);
        JEC_Events.OnMessengerUnreadChanged.RemoveListener(UpdateUnreadIndicator);
    }

    private void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Tab))
        {
            JEC_Events.OnExitMessengerPedestal.Invoke();
        }
    }

    protected override void Interact()
    {
        if (!isActive)
        {
            StartCoroutine(BufferInteract());
        }
    }

    private IEnumerator BufferInteract()
    {
        yield return new WaitForEndOfFrame();

        JEC_Events.OnInteractMessengerPedestal.Invoke();
    }

    private void EnterMessenger()
    {
        isActive = true;

        if (messengerUI != null)
        {
            messengerUI.SetActive(true);
        }

        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    private void ExitMessenger()
    {
        isActive = false;

        if (messengerUI != null)
        {
            messengerUI.SetActive(false);
        }

        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }

    private void UpdateUnreadIndicator(bool hasUnreadMessages)
    {
        if (unreadIndicator != null)
        {
            unreadIndicator.color = hasUnreadMessages ? unreadColor : readColor;
        }
    }
}
