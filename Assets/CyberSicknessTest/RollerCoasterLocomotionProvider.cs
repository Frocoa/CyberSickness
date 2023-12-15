using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerCoasterLocomotionProvider : MonoBehaviour
{

    [SerializeField] private GameObject XROrigin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        XROrigin.transform.Translate(new Vector3(0, 1, 0));
    }
}
