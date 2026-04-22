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
    [SerializeField][Min(0f)] private float _sceneFadeDuration = 1f;
    [SerializeField][Min(0f)] private float _restartFadeTotalDuration = 0.5f;

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
        if (_isRestarting)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_mainMenuSceneName))
        {
            Debug.LogWarning("Main menu scene name is empty.");
            return;
        }

        StartCoroutine(GoToMainMenuRoutine());
    }

    private IEnumerator GoToMainMenuRoutine()
    {
        _isRestarting = true;

        if (_levelFlowManager != null)
        {
            _levelFlowManager.FadeOutBackgroundMusic(_sceneFadeDuration);
        }

        ScreenFadeController.Instance.FadeOutToBlack(_sceneFadeDuration);

        if (_sceneFadeDuration > 0f)
        {
            yield return new WaitForSeconds(_sceneFadeDuration);
        }

        SceneManager.LoadScene(_mainMenuSceneName);
    }

    private IEnumerator RestartRoutine()
    {
        _isRestarting = true;
        PlayRestartAudio();

        float restartHalfFadeDuration = Mathf.Max(0f, _restartFadeTotalDuration * 0.5f);

        if (_levelFlowManager != null)
        {
            _levelFlowManager.FadeOutBackgroundMusic(restartHalfFadeDuration);
        }

        if (restartHalfFadeDuration > 0f)
        {
            ScreenFadeController.Instance.FadeOutToBlack(restartHalfFadeDuration);
            yield return new WaitForSeconds(restartHalfFadeDuration);
        }

        if (restartHalfFadeDuration <= 0f && _restartDelaySeconds > 0f)
        {
            yield return new WaitForSeconds(_restartDelaySeconds);
        }

        if (_levelFlowManager != null)
        {
            _levelFlowManager.RestartCurrentLevel();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (restartHalfFadeDuration > 0f)
        {
            ScreenFadeController.Instance.FadeInFromBlack(restartHalfFadeDuration);
            yield return new WaitForSeconds(restartHalfFadeDuration);
        }

        _isRestarting = false;
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