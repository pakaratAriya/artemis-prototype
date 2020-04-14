using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePosition : MonoBehaviour
{
    public Transform referenceAisel1;
    public Transform referenceAisel2;
    void Start()
    {
        
            
    }

    // Update is called once per frame
    void Update()
    {
        if (referenceAisel1) {
            referenceAisel1.position = transform.position;
        }

        if (referenceAisel2) {
            transform.LookAt(referenceAisel2);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }
}
