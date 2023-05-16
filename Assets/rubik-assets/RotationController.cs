using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    int lastRotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateRotation();   
    }

    void updateRotation()
    {
        Vector3 newRotation = gameObject.transform.rotation.eulerAngles;
        newRotation.y -= lastRotation;
        lastRotation = Controller.frontRotationCount * 90;
        newRotation.y += lastRotation;
        gameObject.transform.rotation = Quaternion.Euler(newRotation);

    }
}
