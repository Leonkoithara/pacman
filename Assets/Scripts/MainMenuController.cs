using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public void PlayGame()
	{
        Debug.Log("starting game");
        SceneManager.LoadScene("StartGame");
    }

	public void QuitGame()
	{
        Application.Quit();
    }
}
