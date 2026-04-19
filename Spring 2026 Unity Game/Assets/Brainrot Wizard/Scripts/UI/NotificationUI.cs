using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    private static NotificationUI _instance;
    public static NotificationUI Instance => _instance;

    public enum NotificationType
    {
        Timer,
        KeyInput,
        CloseButton,
        Confirmation,
        Default
    }

    public struct NotificationData
    {
        public NotificationType type;
        public string title;
        public string subtitle;
        public bool showTimer;
        public float timerDuration;
        public KeyCode keyCode;

        public NotificationData(NotificationType type)
        {
            this.type = type;
            title = "";
            subtitle = "";
            showTimer = false;
            timerDuration = -1f;
            keyCode = KeyCode.None;
        }
    }

    [Header("UI References")]
    public TextMeshProUGUI titleUI;
    public TextMeshProUGUI subtitleUI;
    public TextMeshProUGUI timerUI;
    public Button closeButtonUI;
    public Image backgroundImage;
    
    [Header("Confirmation UI")]
    public Button confirmButtonUI;
    public Button cancelButtonUI;

    private bool? confirmationResult = null;

    private InputAction keyInputAction;
    private bool hasTimer;
    private float currentTimerDuration;

    private Queue<NotificationData> notificationQueue = new();
    private bool isProcessingQueue;

    public bool IsShowing { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        HideUI();
    }

    private void Update()
    {
        if (!hasTimer) return;
        if (currentTimerDuration > 0)
        {
            timerUI.text = Mathf.CeilToInt(currentTimerDuration).ToString();
            currentTimerDuration -= Time.deltaTime;
        }
        else
        {
            HideUI();
        }
    }

    public void ShowNotification(NotificationData data)
    {
        notificationQueue.Enqueue(data);
        if (!isProcessingQueue)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isProcessingQueue = true;
        while (notificationQueue.Count > 0)
        {
            NotificationData current = notificationQueue.Dequeue();
            IsShowing = true;
            backgroundImage.gameObject.SetActive(true);

            switch (current.type)
            {
                case NotificationType.Timer:
                    ShowTimerNotification(current);
                    break;
                case NotificationType.KeyInput:
                    ShowKeyInputNotification(current);
                    break;
                case NotificationType.CloseButton:
                    ShowCloseNotification(current);
                    break;
                case NotificationType.Confirmation:
                    ShowConfirmationNotification(current);
                    break;
                default:
                    ShowDefaultNotification(current);
                    break;
            }

            while (IsShowing)
                yield return null;

            yield return new WaitForSeconds(0.2f);
        }
        backgroundImage.gameObject.SetActive(false);
        isProcessingQueue = false;
    }
    
    public IEnumerator RequestConfirmation(string title, string subtitle, System.Action<bool> callback)
    {
        var data = new NotificationData(NotificationType.Confirmation)
        {
            title = title,
            subtitle = subtitle
        };

        ShowNotification(data);

        while (isProcessingQueue)
            yield return null;

        callback?.Invoke(confirmationResult ?? false);
    }

    private void ShowCloseNotification(NotificationData data)
    {
        DefaultUIFunctionality(data.title, data.subtitle);
        closeButtonUI.gameObject.SetActive(true);
        closeButtonUI.onClick.RemoveAllListeners();
        closeButtonUI.onClick.AddListener(() => IsShowing = false);
    }

    private void ShowTimerNotification(NotificationData data)
    {
        DefaultUIFunctionality(data.title, data.subtitle);
        hasTimer = data.showTimer;
        if (!data.showTimer) return;
        timerUI.gameObject.SetActive(true);
        currentTimerDuration = data.timerDuration;
    }

    private void ShowKeyInputNotification(NotificationData data)
    {
        DefaultUIFunctionality(data.title, $"Press {data.keyCode}");
        keyInputAction?.Disable();
        keyInputAction = new InputAction(type: InputActionType.Button, binding: $"<Keyboard>/{data.keyCode.ToString().ToLower()}");
        keyInputAction.performed += ctx => IsShowing = false;
        keyInputAction.Enable();
    }
    
    private void ShowConfirmationNotification(NotificationData data)
    {
        DefaultUIFunctionality(data.title, data.subtitle);

        confirmButtonUI.gameObject.SetActive(true);
        cancelButtonUI.gameObject.SetActive(true);

        confirmationResult = null;

        confirmButtonUI.onClick.RemoveAllListeners();
        cancelButtonUI.onClick.RemoveAllListeners();

        confirmButtonUI.onClick.AddListener(() =>
        {
            confirmationResult = true;
            IsShowing = false;
        });

        cancelButtonUI.onClick.AddListener(() =>
        {
            confirmationResult = false;
            IsShowing = false;
        });
    }

    private void ShowDefaultNotification(NotificationData data)
    {
        DefaultUIFunctionality(data.title, data.subtitle);
    }

    private void DefaultUIFunctionality(string titleText = "", string subtitleText = "")
    {
        backgroundImage.gameObject.SetActive(true);
        IsShowing = true;
        titleUI.gameObject.SetActive(true);
        subtitleUI.gameObject.SetActive(true);
        timerUI.gameObject.SetActive(false);
        closeButtonUI.gameObject.SetActive(false);
        confirmButtonUI.gameObject.SetActive(false);
        cancelButtonUI.gameObject.SetActive(false);
        titleUI.text = string.IsNullOrWhiteSpace(titleText) ? "Invalid Notification" : titleText;
        subtitleUI.text = subtitleText;
    }

    public void HideUI()
    {
        IsShowing = false;
        hasTimer = false;
        
        timerUI.gameObject.SetActive(false);
        closeButtonUI.gameObject.SetActive(false);
        
        confirmButtonUI.gameObject.SetActive(false);
        cancelButtonUI.gameObject.SetActive(false);
        
        titleUI.gameObject.SetActive(false);
        subtitleUI.gameObject.SetActive(false);

        if (keyInputAction == null) return;
        keyInputAction.Disable();
        keyInputAction = null;
    }
}