using UnityEngine;
using System.Collections.Generic;

public class CharacterControl : IControl
{
	[HideInInspector] public bool selected;
		
	private void clamp_height() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit) 
			|| Physics.Raycast (transform.position, Vector3.up, out hit))
		{
			transform.position = hit.point + Vector3.up * .1f;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		clamp_height ();
		
		if (!selected) return;
		
		foreach (IAbility a in abilities)
		{
			if (Input.GetMouseButtonDown(1)) a.on_rmouse();
			if (Input.GetKeyDown (KeyCode.Space)) a.on_space ();
		}

	}

}
