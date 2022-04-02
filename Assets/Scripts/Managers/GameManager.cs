using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        EnableOrDisableSpawners();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update() {
      EnableOrDisableSpawners();
    }
    
    void EnableOrDisableSpawners() {
      GameObject player = GameObject.FindGameObjectWithTag("Player");

      if (player == null) DisableSpawners();
      else EnableSpawners();
      
    }

    EnemySpawner GetEnemySpawner() {
      return GetComponent<EnemySpawner>();
    }

    ItemSpawner GetItemSpawner() {
      return GetComponent<ItemSpawner>();
    }

    void DisableSpawners() {
      GetEnemySpawner().enabled = false;
      GetItemSpawner().enabled = false;
    }

    void EnableSpawners() {
      GetEnemySpawner().enabled = true;
      GetItemSpawner().enabled = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      DisableSpawners();
    }

}
