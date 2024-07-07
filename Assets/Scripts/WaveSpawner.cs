using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject wavePrefab;
    [SerializeField] private float period;

    private float currentTime;
    
    

    

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
            
        if (currentTime > period)
        {
            currentTime = 0;
            Instantiate(wavePrefab, transform.position, transform.rotation);
        }
    }
}
