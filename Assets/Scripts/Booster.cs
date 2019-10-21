using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

   
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
           // float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {

                audioSource.Play();
            }
        }
        else audioSource.Stop();
    }

    private void Rotate()
    {
       
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidbody.freezeRotation = false;

    }
}
