using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections;

public class Restart : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string _mainMenuSceneName = "MainMenu";

    [Header("Input")]
    [SerializeField] private bool _enableEscapeToMenu = true;
    [SerializeField] private bool _enableRestartKey = true;
    [SerializeField] private KeyCode _restartKey = KeyCode.R;
    [SerializeField][Min(0f)] private float _restartDelaySeconds = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioSource _restartAudioSource;
    [SerializeField] private AudioClip _restartClip;
    [SerializeField][Range(0f, 1f)] private float _restartVolume = 1f;

    private LevelFlowManager _levelFlowManager;
    private bool _isRestarting;

    private void Awake()
    {
        _levelFlowManager = FindFirstObjectByType<LevelFlowManager>();
        EnsureRestartAudioSource();
    }

    private void Update()
    {
        if (_enableRestartKey && !_isRestarting && Input.GetKeyDown(_restartKey))
        {
            RestartGame();
            return;
        }

        if (!_enableEscapeToMenu || _isRestarting)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }

    public void RestartGame()
    {
        if (_isRestarting)
        {
            return;
        }

        StartCoroutine(RestartRoutine());
    }

    public void GoToMainMenu()
    {
        if (string.IsNullOrWhiteSpace(_mainMenuSceneName))
        {
            Debug.LogWarning("Main menu scene name is empty.");
            return;
        }

        SceneManager.LoadScene(_mainMenuSceneName);
    }

    private IEnumerator RestartRoutine()
    {
        _isRestarting = true;
        PlayRestartAudio();

        if (_restartDelaySeconds > 0f)
        {
            yield return new WaitForSeconds(_restartDelaySeconds);
        }

        if (_levelFlowManager != null)
        {
            _levelFlowManager.RestartCurrentLevel();
            _isRestarting = false;
            yield break;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void EnsureRestartAudioSource()
    {
        if (_restartAudioSource == null && _restartClip != null)
        {
            _restartAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (_restartAudioSource == null)
        {
            return;
        }

        _restartAudioSource.playOnAwake = false;
        _restartAudioSource.loop = false;
        _restartAudioSource.spatialBlend = 0f;
    }

    private void PlayRestartAudio()
    {
        if (_restartClip == null)
        {
            return;
        }

        EnsureRestartAudioSource();
        if (_restartAudioSource == null)
        {
            return;
        }

        _restartAudioSource.PlayOneShot(_restartClip, _restartVolume);
    }
}