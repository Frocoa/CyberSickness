using System;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject door;


    private void Update()
    {
        transform.Translate(0, Mathf.Cos(Time.time) * 0.004f, Mathf.Sin(Time.time) * 0.004f);
    }

    private void OnCollisionEnter (Collision _) {
        Destroy(gameObject);
        Destroy(door);
    }
}
