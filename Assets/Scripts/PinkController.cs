using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkController : MonoBehaviour
{
	GhostBehaviour gb;

	public GameObject Pacman;
	
    void Start()
    {
		gb = gameObject.GetComponent<GhostBehaviour>();
    }

    void Update()
    {
		if(!GameController.GamePaused)
		{
			gb.move_ghost();
		}
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		Vector3 chase_point = determine_chasept();
		gb.ghost_collision(other, chase_point);
	}

	Vector3 determine_chasept()
	{
		Vector3 chase_point = new Vector3(0.0f, 0.0f, 0.0f);
		
		return chase_point;
	}
}
