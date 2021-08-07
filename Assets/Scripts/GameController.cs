using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoreObject
{
	public int _score;
	public string _name;

	public ScoreObject(string name, int score)
	{
		_name = name;
		_score = score;
	}
}

public static class HighScoreManager
{
	public static void SaveScore(string name, int score)
	{
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/pacman_highscores";

        FileStream stream = new FileStream(path, FileMode.Create);
        ScoreObject score_object = new ScoreObject(name, score);

        formatter.Serialize(stream, score_object);
        stream.Close();
    }

	public static ScoreObject LoadScore()
	{
		string path = Application.persistentDataPath + "/pacman_highscores";

		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			ScoreObject data = formatter.Deserialize(stream) as ScoreObject;

			stream.Close();

			return data;
        }
		else
        {
            Debug.LogError("Error: Save file not found in " + path);
            return null;
		}
	}
}

public class GameController : MonoBehaviour
{
	public static bool GamePaused;
	public static int Probablity;
	public static float Speed;

	public GameObject PelletManager;
	public GameObject SpeedPills;

	public GameObject Pacman;
	public GameObject[] Ghosts;
	
	public Text LevelText;
	public Text LivesText;
	public Text ScoreText;
    public Text HighScoreText;

    int level;
	int lives;
	int score;

	int offset;

	int pellets;
	int max_pellets;
	
    void Start()
    {
        level = 1;
		lives = 3;
		score = 0;
		pellets = 0;
		max_pellets = 145;
		offset = 0;
		GamePaused = true;
		Probablity = 10;
		Speed = 0.04f;

        ScoreObject so = HighScoreManager.LoadScore();
		if(so != null)
            HighScoreText.text = "High Score: " + so._score;
		else
			HighScoreText.text = "High Score: 0";

        LevelText.text = "Level: " + level;
		LivesText.text = "Lives: " + lives;
		ScoreText.text = "Score: " + score;

		foreach (GameObject ghost in Ghosts)
		{
			ghost.GetComponent<GhostBehaviour>().reset_state(false);
		}
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
			GamePaused = !GamePaused;
		}
    }

	public void increase_score(int score_type)				//0: pellet
	{
		if(score_type == 0)
		{
			pellets++;
			score += level * 10;
			
			if(pellets >= max_pellets)
			{
				next_level();
			}
		}
		if(score_type == 1)
		{
			score += level * level * 20 + offset * level * 20;
		}

		ScoreText.text = "Score: " + score;
	}

    public void next_level()
    {
		GamePaused = true;
        level++;
		pellets = 0;
		LevelText.text = "Level: " + level;

		if(Speed < 0.07f)
			Speed += 0.005f;
		if(Probablity < 50)
			Probablity += 5;
		Pacman.GetComponent<PacmanController>().Speed += 0.005f;

		foreach (Transform child in PelletManager.transform)
		{
			child.gameObject.SetActive(true);
		}
		foreach (Transform child in SpeedPills.transform)
		{
			child.gameObject.SetActive(true);
		}

		foreach (GameObject ghost in Ghosts)
		{
			ghost.GetComponent<GhostBehaviour>().reset_state(false);
		}
		Pacman.GetComponent<PacmanController>().reset_state();
    }

	public void PacmanDeath()
	{
		GamePaused = true;
		
        lives--;

		if(lives == 0)
		{
			ScoreObject so = HighScoreManager.LoadScore();
			if(so == null || so._score < score)
			{
				HighScoreManager.SaveScore("Leon", score);
                HighScoreText.text = "High Score: " + score;
            }
			
			level = 1;
			lives = 3;
			score = 0;
			pellets = 0;
			Probablity = 10;
			Speed = 0.04f;
			Pacman.GetComponent<PacmanController>().Speed = 0.04f;

			foreach (Transform child in PelletManager.transform)
			{
				child.gameObject.SetActive(true);
			}
			foreach (Transform child in SpeedPills.transform)
			{
				child.gameObject.SetActive(true);
			}
		}
		LivesText.text = "Lives: " + lives;
		LevelText.text = "Level: " + level;
		ScoreText.text = "Score: " + score;

		foreach (GameObject ghost in Ghosts)
		{
			ghost.GetComponent<GhostBehaviour>().reset_state(false);
		}

		Pacman.GetComponent<PacmanController>().reset_state();
	}

	public void stimulate()
	{
		pellets++;
		if(pellets >= max_pellets)
		{
			next_level();
		}
		offset = 0;
		foreach (GameObject ghost in Ghosts)
		{
			StartCoroutine(ghost.GetComponent<GhostBehaviour>().scare());
		}
	}

	public void GhostDeath(GameObject ghost)
	{
		increase_score(1);
		StopCoroutine(ghost.GetComponent<GhostBehaviour>().scare());
		StartCoroutine(ghost.GetComponent<GhostBehaviour>().death());
	}
}
