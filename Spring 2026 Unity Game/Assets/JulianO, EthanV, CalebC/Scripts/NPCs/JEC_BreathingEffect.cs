using UnityEngine;

public class JEC_BreathingEffect : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    [SerializeField] float expandDuration = 1.0f;
    private float currentTime = 0f;
    [SerializeField] Vector3 breatheIn;
    [SerializeField] Vector3 breatheOut;
    private bool breathingIn = true;

    [SerializeField] bool pulsing = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (targetObject == null)
        {
            targetObject = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        BreatheUpdate();
    }

    private void BreatheUpdate()
    {
        if (pulsing)
        {
            Vector3 targetScale = breathingIn ? breatheIn : breatheOut;
            Vector3 startScale = breathingIn ? breatheOut : breatheIn;

            currentTime += Time.deltaTime;
            float lerpFactor = currentTime / expandDuration;

            targetObject.transform.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);
            
            if (lerpFactor >= 1.0f)
            {
                breathingIn = !breathingIn;
                currentTime = 0f;
            }
        }
    }
}
