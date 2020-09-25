using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller : MonoBehaviour
{
	public static bool game_paused;
	public static bool game_over;

	public static int probablity;
	public static float increment;

    public GameObject pelletManager;
	public GameObject speedPills;

	public GameObject pacman;
	public GameObject pink;
	public GameObject red;
	public GameObject green;
	public GameObject blue;

    public int level;
	public int lives;

    void Start()
    {
        level = 1;
		lives = 3;
		game_paused = true;
		game_over = false;
		probablity = 10;
		increment = 0.06f;
    }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			game_paused = !game_paused;
		}
	}

    public void next_level()
    {
		game_paused = true;
        level++;
		increment += 0.01f;
		if(probablity < 50)
			probablity += 5;

		foreach (Transform child in pelletManager.transform)
		{
			child.gameObject.SetActive(true);
		}
		foreach (Transform child in speedPills.transform)
		{
			child.gameObject.SetActive(true);
		}

		reset_level();
    }

	public void restart_level()
	{
		game_paused = true;
		game_over = true;

		lives--;

		if(lives == 0)
		{
			level = 1;
			lives = 3;
			probablity = 10;
			increment = 0.06f;

			foreach (Transform child in pelletManager.transform)
			{
				child.gameObject.SetActive(true);
			}
			foreach (Transform child in speedPills.transform)
			{
				child.gameObject.SetActive(true);
			}			
		}

		reset_level();

		game_over = false;
	}

	void reset_level()
	{
		green.GetComponent<GreenController>().reset_level();
		red.GetComponent<RedController>().reset_level();
		blue.GetComponent<BlueController>().reset_level();
		pink.GetComponent<PinkController>().reset_level();
	}

	public void stimulant()
	{
		StartCoroutine(green.GetComponent<GreenController>().scare());
		StartCoroutine(red.GetComponent<RedController>().scare());
		StartCoroutine(blue.GetComponent<BlueController>().scare());
		StartCoroutine(pink.GetComponent<PinkController>().scare());		
	}

	public void death(GameObject ghost)
	{
		switch(ghost.name)
		{
			case "green":
//				 StopCoroutine(green.GetComponent<GreenController>().scare());
				 StartCoroutine(ghost.GetComponent<GreenController>().death());
				 break;
			case "red":
//				 StopCoroutine(red.GetComponent<RedController>().scare());
				 StartCoroutine(ghost.GetComponent<RedController>().death());
				 break;
			case "blue":
//				 StopCoroutine(blue.GetComponent<BlueController>().scare());
				 StartCoroutine(ghost.GetComponent<BlueController>().death());
				 break;
			case "pink":
//				 StopCoroutine(pink.GetComponent<PinkController>().scare());
				 StartCoroutine(pink.GetComponent<PinkController>().death());
				 break;				 
		}
	}

	public static int get_probablity()
	{
		return probablity;
	}
	
	public static float get_increment()
	{
		return increment;
	}	
}
