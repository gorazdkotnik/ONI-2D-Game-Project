using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        QualitySettings.SetQualityLevel(PlayerPrefs.HasKey("quality") ? PlayerPrefs.GetInt("quality") : 0);
        Screen.fullScreen = PlayerPrefs.HasKey("fullscreen") ? PlayerPrefs.GetInt("fullscreen") == 1 : false;
    }

}
