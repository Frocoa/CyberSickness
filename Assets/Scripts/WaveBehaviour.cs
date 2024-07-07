using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 2;

    [SerializeField] private Vector3 speed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);   
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime);
    }
}
