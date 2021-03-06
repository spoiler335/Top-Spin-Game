using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 3600;
    public bool doSpin = false;

    private Rigidbody rb;
    public GameObject plyerGraphics;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(doSpin)
        {
            plyerGraphics.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

        }
    }

}
