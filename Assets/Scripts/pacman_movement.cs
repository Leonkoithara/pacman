using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pacman_movement : MonoBehaviour
{
    int xdirection;
    int ydirection;
    int input;
	int level;
	int lives;

    int stop_node_flag;

    int max_pellets;
    int pellets;

	public int offset;
	public bool stimulated;    
    public float increment;
	public int score;

	public Text lives_text;
	public Text score_text;
	public Text level_text;
	
    public GameObject GameController;
    
    void Start()
    {
		level = 1;
		lives = 3;
        pellets = 0;
		max_pellets = 145;
		input = 1;
        xdirection = 1;
        ydirection = 0;
        stop_node_flag = -1;
		offset = 0;
		stimulated = false;
    }

    void Update()
    {
		lives_text.text = "Lives: " + lives;
		score_text.text = "Score: " + score;
		level_text.text = "Level: " + level;
		
		if(!game_controller.game_paused)
		{
	        transform.position += new Vector3(xdirection*increment, ydirection*increment, 0);
	
		    if (Input.GetKeyDown("up"))
			{
				input = 2;
	    
				if(ydirection == -1)
		    	{
					transform.localEulerAngles = new Vector3(0, 0, 90);
	        		ydirection = 1;
		    	}
				else if(stop_node_flag != -1)
	    		{
					if((stop_node_flag & 2) != 0)
					{
						transform.localEulerAngles = new Vector3(0, 0, 90);
			    		ydirection = 1;
	            		stop_node_flag = -1;
					}
	    		}    
			}

			else if (Input.GetKeyDown("down"))
			{
				input = 8;

	    		if(ydirection == 1)
	    		{
					transform.localEulerAngles = new Vector3(0, 0, -90);
	        		ydirection = -1;
	    		}
				else if(stop_node_flag != -1)
	    		{
					if((stop_node_flag & 8) != 0)
					{
						transform.localEulerAngles = new Vector3(0, 0, -90);
			    		ydirection = -1;
	            		stop_node_flag = -1;
					}
	    		}
			}
	
			else if (Input.GetKeyDown("left"))
			{
				input = 4;

	    		if(xdirection == 1)
	    		{
					transform.localEulerAngles = new Vector3(0, 0, 180);
					xdirection = -1;
	    		}
				else if(stop_node_flag != -1)
	    		{
					if((stop_node_flag & 4) != 0)
					{
						transform.localEulerAngles = new Vector3(0, 0, 180);
			    		xdirection = -1;
	            		stop_node_flag = -1;
					}
	    		}
			}
	
			else if (Input.GetKeyDown("right"))
			{
				input = 1;

	    		if(xdirection == -1)
	    		{
					transform.localEulerAngles = new Vector3(0, 0, 0);
						xdirection = 1;
	    		}
				else if(stop_node_flag != -1)
	    		{
					if((stop_node_flag & 1) != 0)
					{
						transform.localEulerAngles = new Vector3(0, 0, 0);
						xdirection = 1;
            			stop_node_flag = -1;
					}
	    		}	    
			}
		}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if(other.tag == "node")
		{
			transform.position = other.transform.position;
			
       	    stop_node_flag = other.gameObject.GetComponent<node_behaviour>().node_flag;

	    	if((stop_node_flag & input) != 0)
	    	{
	    	    stop_node_flag = -1;
	    
			    switch(input)
	    		{
	        		case 1:
		        		transform.localEulerAngles = new Vector3(0, 0, 0);
						xdirection = 1;
		       			ydirection = 0;
		        		break;
		    		case 2:
						 transform.localEulerAngles = new Vector3(0, 0, 90);
		    			 xdirection = 0;
		    	 		 ydirection = 1;
		    	 		 break;
		    		case 4:
		    	 		 transform.localEulerAngles = new Vector3(0, 0, 180);		
		    	 		 xdirection = -1;
		    	 		 ydirection = 0;
		    	 		 break;
		    		case 8:
		    	 		 transform.localEulerAngles = new Vector3(0, 0, -90);		
		    	 		 xdirection = 0;
		    	 		 ydirection = -1;
		    	 		 break;
	        	}
	    	}
	    	else
	    	{
	    	    xdirection = 0;
	    		ydirection = 0;
	    	}
		}
		else if(other.tag == "pellet")
		{
			other.gameObject.SetActive(false);
	    	pellets++;
			score += level * 100;

	    	if(pellets == max_pellets)
	    	{
				transform.position = new Vector3(0, -1.76f, 0);
				transform.localEulerAngles = new Vector3(0, 0, 0);
				level++;
				xdirection = 1;
				ydirection = 0;
				input = 1;
				increment += 0.01f;
				pellets = 0;
	        	GameController.GetComponent<game_controller>().next_level();
	   		}
		}

		else if(other.tag == "teleporter")
		{
			transform.position = new Vector3((transform.position.x - (Mathf.Sign(transform.position.x) * 0.05f)) * -1, transform.position.y, 0);
		}

		else if(other.tag == "stimulant")
		{
			other.gameObject.SetActive(false);
	    	pellets++;

	    	if(pellets == max_pellets)
	    	{
				transform.position = new Vector3(0, -1.76f, 0);
				transform.localEulerAngles = new Vector3(0, 0, 0);
				level++;
				xdirection = 1;
				ydirection = 0;
				input = 1;
				increment += 0.01f;
				pellets = 0;
	        	GameController.GetComponent<game_controller>().next_level();
	   		}
			else
			{
				stimulated = true;
				GameController.GetComponent<game_controller>().stimulant();
			}
		}

		else if(other.tag == "ghost")
		{
			if(!stimulated)
			{
				lives--;
				if(lives == 0)
				{
					lives = 3;
					level = 1;
					score = 0;
				}
				xdirection = 1;
				ydirection = 0;
				input = 1;
				transform.position = new Vector3(0.0f, -1.76f, 0.0f);
				transform.localEulerAngles = new Vector3(0, 0, 0);
			
				GameController.GetComponent<game_controller>().restart_level();
			}
			else
			{
				offset++;
				score += level * offset * 100;
				GameController.GetComponent<game_controller>().death(other.gameObject);
			}
		}
    }
}
