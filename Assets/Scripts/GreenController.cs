using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenController : MonoBehaviour
{
	int xdirection;
	int ydirection;
	Vector3 chase_point, pacman_offset;
	Vector3 home_position;
	bool horizon, up;
	
	public bool inside_house;
	public GameObject pacman;
	public Sprite not_scared;
	public Sprite scared;
	public float increment;
	public int probablity;

    void Start()
    {
		inside_house = true;
		horizon = true;
		up = true;
		home_position = new Vector3(-0.58f, 0.84f, 0.0f);
		pacman_offset = new Vector3(1.0f, 1.0f, 0);
		chase_point = pacman.transform.position + pacman_offset;
    }

    void Update()
    {
		if(inside_house && !game_controller.game_paused)
		{
			if(transform.position.x < 0 && horizon)
				transform.position = new Vector3(transform.position.x + increment, transform.position.y, 0);
			else
				horizon = false;

			if(transform.position.y < 1.78 && !horizon && up)
				transform.position = new Vector3(transform.position.x, transform.position.y + increment, 0);
			else if(!horizon)
				up = false;

			if(!horizon && !up)
			{
				int select = Random.Range(0, 2);
				if(select == 0)
				{
					xdirection = 1;
					ydirection = 0;
				}
				else
				{
					xdirection = -1;
					ydirection = 0;
				}
				inside_house = false;
			}
		}

		if(!inside_house && !game_controller.game_paused)
		{
			float pacman_direction = pacman.transform.localEulerAngles.z;

			switch(pacman_direction)
			{
				case 0:
					 pacman_offset.x = 1.0f;
					 pacman_offset.y = 1.0f;
					 break;
				case 90:
					 pacman_offset.x = -1.0f;
					 pacman_offset.y = 1.0f;
					 break;
				case 180:
					 pacman_offset.x = -1.0f;
					 pacman_offset.y = -1.0f;
					 break;
				case 270:
					 pacman_offset.x = 1.0f;
					 pacman_offset.y = -1.0f;
					 break;
			}
			
			chase_point = pacman.transform.position + pacman_offset;
			transform.position += new Vector3(xdirection*increment, ydirection*increment, 0);
		}
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "node")
		{
			transform.position = other.transform.position;
			
			int nf, select, dir = 0;

			nf = other.gameObject.GetComponent<node_behaviour>().node_flag;

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
	}

	public void reset_level()
	{
		transform.position = home_position;
		inside_house = true;
		horizon = true;
		up = true;
	}
	public IEnumerator scare()
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = scared;
		probablity = 0;
		increment = 0.04f;

		yield return new WaitForSeconds(10);

		gameObject.GetComponent<SpriteRenderer>().sprite = not_scared;
		probablity = game_controller.get_probablity();
		increment = game_controller.get_increment();
	}

	public IEnumerator death()
	{
		transform.position = home_position;
		inside_house = true;
		horizon = true;
		up = true;
		probablity = game_controller.get_probablity();
		increment = 0.0f;

		yield return new WaitForSeconds(3);

		increment = game_controller.get_increment();
	}
}

