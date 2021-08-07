using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueController : MonoBehaviour
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
		Vector3 pacman_offset = new Vector3(1.0f, 1.0f, 0.0f);
		float pacman_direction = Pacman.transform.localEulerAngles.z;

		switch(pacman_direction)
		{
			case 0:
				 pacman_offset.x = 1.0f;
				 pacman_offset.y = 0.0f;
				 break;
			case 90:
				 pacman_offset.x = 0.0f;
				 pacman_offset.y = 1.0f;
				 break;
			case 180:
				 pacman_offset.x = -1.0f;
				 pacman_offset.y = 0.0f;
				 break;
			case 270:
				 pacman_offset.x = 0.0f;
				 pacman_offset.y = -1.0f;
				 break;
		}
			
		chase_point = Pacman.transform.position + pacman_offset;

		return chase_point;
	}
}
