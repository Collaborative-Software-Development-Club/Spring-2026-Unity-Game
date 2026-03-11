using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystem : MonoBehaviour
{

    [SerializeField] GameObject vfx;
    Vector3 clickPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkClick();
    }

    void checkClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newParticle = Instantiate(vfx, clickPosition, Quaternion.identity);
            Destroy(newParticle, 1);
        }

        
    }
}
