using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
	int xdirection;
	int ydirection;
	float speed;
	int probablity;
	bool inside_house;
	bool horizon;
	
	public Vector3 HomePosition;
	public bool Scared;
	public GameObject gameController;
	public Sprite ScaredSprite;
	public Sprite NotScaredSprite;

    public void reset_state(bool go_home)
    {
		if(!go_home)
			transform.position = HomePosition;

		speed = GameController.Speed;

		probablity = GameController.Probablity;
		ydirection = 0;
		Scared = false;
		inside_house = true;
		horizon = true;
		
		gameObject.GetComponent<SpriteRenderer>().sprite = NotScaredSprite;
		
		int select = Random.Range(0, 2);
		if(select == 0)
			xdirection = 1;
		else
			xdirection = -1;
    }

    public void move_ghost()
    {
		if(inside_house)
			exit_house();
		else
			transform.position += new Vector3(xdirection*speed, ydirection*speed, 0);
    }

	void exit_house()
	{
		if(transform.position.x < -0.07f)
			transform.position += new Vector3(speed, 0, 0);
		else if(transform.position.x > 0.07f)
			transform.position -= new Vector3(speed, 0, 0);
		else
			horizon = false;

		if(transform.position.y < 1.78f && !horizon)
			transform.position += new Vector3(0, speed, 0);
		else if(!horizon)
			inside_house = false;
	}

	public void ghost_collision(Collider2D other, Vector3 chase_point)
	{
		if(other.tag == "node")
		{
			transform.position = other.transform.position;

			int nf, select, dir = 0;

			nf = other.gameObject.GetComponent<NodeBehaviour>().NodeFlag;
			while((dir & nf) == 0)
			{
				select = Random.Range(0, 101);

				int tempx, tempy;
				if(chase_point.x > transform.position.x)
					tempx = 1;
				else
					tempx = -1;
				if(chase_point.y > transform.position.y)
					tempy = 1;
				else
					tempy = -1;
					
				if(select <= 25+(tempx*probablity/2))
					select = 0;
				else if(select <= 50-(tempx*probablity/2))
					select = 2;
				else if(select <= 75+(tempy*probablity/2))
					select = 1;
				else
					select = 3;

				dir = (int)Mathf.Pow(2, select);
			}

			switch(dir)
			{
				case 1:
					 xdirection = 1;
					 ydirection = 0;
					 break;
				case 2:
					 xdirection = 0;
					 ydirection = 1;
					 break;
				case 4:
					 xdirection = -1;
					 ydirection = 0;
					 break;
				case 8:
					 xdirection = 0;
					 ydirection = -1;
					 break;
			}
		}

		else if(other.tag == "teleporter")
		{
			transform.position = new Vector3((transform.position.x - (Mathf.Sign(transform.position.x) * 0.05f)) * -1, transform.position.y, 0);			
		}

		else if(other.tag == "pacman")
		{
			if(!Scared)
			{
				gameController.GetComponent<GameController>().PacmanDeath();
			}
			else
			{
				gameController.GetComponent<GameController>().GhostDeath(gameObject);
			}
		}
	}

	public IEnumerator scare()
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = ScaredSprite;
		Scared = true;
		probablity = 0;
		speed = 0.04f;

		yield return new WaitForSeconds(10);

		gameObject.GetComponent<SpriteRenderer>().sprite = NotScaredSprite;
		probablity = GameController.Probablity;
		speed = GameController.Speed;
		Scared = false;
	}

	public IEnumerator death()
	{
		speed = 0.0f;
		transform.position = new Vector3(HomePosition.x, 0.84f, 0);
		
		yield return new WaitForSeconds(3);

		reset_state(true);
	}
}
