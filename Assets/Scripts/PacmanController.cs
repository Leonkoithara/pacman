using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
	int xdirection;
	int ydirection;
    int input;

    int stop_node_flag;

    public float Speed;
	
    public GameObject gameController;
    
    void Start()
    {
		input = 1;
        xdirection = 1;
        ydirection = 0;
        stop_node_flag = -1;
    }

    void Update()
    {
		if(!GameController.GamePaused)
		{
	        transform.position += new Vector3(xdirection*Speed, ydirection*Speed, 0);
	
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
			
       	    stop_node_flag = other.gameObject.GetComponent<NodeBehaviour>().NodeFlag;

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
			gameController.GetComponent<GameController>().increase_score(0);
		}

		else if(other.tag == "teleporter")
		{
			transform.position = new Vector3((transform.position.x - (Mathf.Sign(transform.position.x) * 0.05f)) * -1, transform.position.y, 0);
		}

		else if(other.tag == "stimulant")
		{
			other.gameObject.SetActive(false);
			gameController.GetComponent<GameController>().stimulate();
		}
    }

	public void reset_state()
	{
		xdirection = 1;
		ydirection = 0;
		input = 1;
		transform.position = new Vector3(0, -1.76f, 0);
		transform.localEulerAngles = new Vector3(0, 0, 0);
		stop_node_flag = -1;
	}
}
