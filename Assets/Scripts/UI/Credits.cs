using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    void Start()
    {
        Invoke("ExitCredits", 20f);
    }

    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            ExitCredits();
        }
    }

    void ExitCredits()
    {
        SceneManager.LoadScene(0);
    }
}
