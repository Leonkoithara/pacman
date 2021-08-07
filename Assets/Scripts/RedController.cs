using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedController : MonoBehaviour
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
		Vector3 chase_point;
		
		chase_point = Pacman.transform.position;

		return chase_point;
	}
}
