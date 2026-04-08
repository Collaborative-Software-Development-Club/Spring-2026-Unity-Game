using System.Collections.Generic;
using UnityEngine;

public class JEC_PageManager : MonoBehaviour
{
    public static JEC_PageManager Instance { get; private set; }

    [SerializeField] private List<JEC_PageData> pages;
    [SerializeField] private Transform pageParent;
    [SerializeField] private JEC_PageData defaultPage;

    private Dictionary<string, JEC_PageData> pageLookup;
    private Dictionary<JEC_PageData, GameObject> pageInstances;
    private GameObject activePageInstance;
    private JEC_Page activePageComponent;
    private JEC_PageData activePageData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        pageLookup = new Dictionary<string, JEC_PageData>();
        pageInstances = new Dictionary<JEC_PageData, GameObject>();
        foreach (var page in pages)
        {
            if (page == null || string.IsNullOrEmpty(page.url))
                continue;

            pageLookup[page.url.ToLower()] = page;
        }
    }

    private void Start()
    {
        JEC_Events.OnEnterURL.AddListener(NavigateToURL);

        if (defaultPage != null)
            LoadPage(defaultPage);
    }

    private void OnDestroy()
    {
        JEC_Events.OnEnterURL.RemoveListener(NavigateToURL);

        if (Instance == this)
            Instance = null;
    }

    public bool IsValidURL(string url)
    {
        return pageLookup.ContainsKey(url.ToLower());
    }

    public void NavigateToURL(string url)
    {
        string key = url.ToLower();

        if (!pageLookup.TryGetValue(key, out JEC_PageData pageData))
        {
            Debug.LogWarning("JEC_WARNING: No page registered for URL: " + url);
            return;
        }

        LoadPage(pageData);
    }

    private void LoadPage(JEC_PageData pageData)
    {
        if (pageData == null)
            return;

        if (activePageData == pageData && activePageInstance != null)
        {
            JEC_Events.OnPageChanged.Invoke(pageData);
            return;
        }

        UnloadCurrentPage();

        if (!pageInstances.TryGetValue(pageData, out activePageInstance) || activePageInstance == null)
        {
            if (pageData.contentPrefab == null)
            {
                Debug.LogError("JEC_ERROR: PageData '" + pageData.pageName + "' has no content prefab assigned.");
                return;
            }

            Transform parent = pageParent != null ? pageParent : transform;
            activePageInstance = Instantiate(pageData.contentPrefab, parent);
            activePageInstance.transform.localPosition = Vector3.zero;
            activePageInstance.transform.localRotation = Quaternion.identity;
            activePageInstance.transform.localScale = Vector3.one;
            pageInstances[pageData] = activePageInstance;
        }

        activePageInstance.SetActive(true);
        activePageComponent = activePageInstance.GetComponent<JEC_Page>();
        if (activePageComponent != null)
            activePageComponent.OnPageLoad(pageData);

        activePageData = pageData;
        JEC_Events.OnPageChanged.Invoke(pageData);
    }

    private void UnloadCurrentPage()
    {
        if (activePageInstance == null)
            return;

        if (activePageComponent != null)
            activePageComponent.OnPageUnload();

        activePageInstance.SetActive(false);
        activePageInstance = null;
        activePageComponent = null;
        activePageData = null;
    }
}
