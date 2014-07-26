using UnityEngine;
using System.Collections.Generic;


/********************************************************
 * CharacterControl - implements the IControl inter-	*
 * face - see for details								*
 * 														*
 * An interface for controlling a character and its		*
 * abilities.											*
 * 														*
 * Maintains a list of abilities as per IControl, and	*
 * updates them based on the keyboard and mouse			*
 * inputs. Also maintains some helper functions for 	*
 * controlling the character's movements.				*
 ********************************************************/
public class CharacterControl : IControl
{
	[HideInInspector] public bool selected;
	
	/**
	 * clamp_height() - maintains the player just above ground level
	 * called every frame
	 * 
	 * TODO: may need to be able to disable this...
	 */
	private void clamp_height()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit) 
			|| Physics.Raycast (transform.position, Vector3.up, out hit))
		{
			transform.position = hit.point + Vector3.up * .1f;
		}
	}

	// Use this for initialization
	void Start () {
		add_nav_mesh_agent ();
	}

	// Update is called once per frame
	void Update () {
		clamp_height ();
		
		if (!selected) return;
		
		// TODO: add buttons to this as abilities require
		foreach (IAbility a in abilities)
		{
			if (Input.GetMouseButtonDown(1)) a.on_rmouse ();
			if (Input.GetMouseButtonDown(0)) a.on_lmouse ();
			if (Input.GetKeyDown (KeyCode.Space)) a.on_space ();
			if (Input.GetKeyDown (KeyCode.Q)) a.on_qkey ();
			if (Input.GetKeyDown (KeyCode.W)) a.on_wkey ();
		}
	}
}
