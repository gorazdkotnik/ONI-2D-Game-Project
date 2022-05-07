using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame(string difficulty)
  {
    SceneManager.LoadScene("Game" + difficulty);
  }

  public void Credits()
  {
    SceneManager.LoadScene("Credits");
  }

  public void QuitGame()
  {
    Debug.Log("QUIT");
    Application.Quit();
  }
}
