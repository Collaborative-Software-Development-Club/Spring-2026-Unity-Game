using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystem : MonoBehaviour
{

    [SerializeField] GameObject vfx;
    Vector3 clickPosition;
    bool clickable = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckClick();
    }

    void CheckClick()
    {
        if (Input.GetMouseButtonDown(0) && clickable == true)
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newParticle = Instantiate(vfx, clickPosition, Quaternion.identity);
            Destroy(newParticle, 0.3f);
            clickable = false;
            Invoke(nameof(Reload), 0.3f);
        }

    }

    void Reload()
    {
        clickable = true;
    }
}
