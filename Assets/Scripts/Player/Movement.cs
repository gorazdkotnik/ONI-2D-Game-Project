using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float horizontalSpeed = 10f;
    [SerializeField] float verticalSpeed = 10f;

    
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput > 0.1f || horizontalInput < -0.1f)
        {
            transform.Translate(horizontalInput * Time.deltaTime * horizontalSpeed, 0f, 0f);
        }

        if (verticalInput > 0.1f || verticalInput < -0.1f)
        {
            transform.Translate(0f, verticalInput * Time.deltaTime * verticalSpeed, 0f);
        }
    }
}
