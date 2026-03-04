using UnityEngine;

public class JEC_Page : MonoBehaviour
{
    [SerializeField] private JEC_PageData pageData;

    public JEC_PageData PageData => pageData;

    public virtual void OnPageLoad(JEC_PageData data)
    {
        pageData = data;
    }

    public virtual void OnPageUnload()
    {
    }
}
