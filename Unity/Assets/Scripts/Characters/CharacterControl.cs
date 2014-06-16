using UnityEngine;
using System.Collections.Generic;

public class CharacterControl : IControl
{
	[HideInInspector] public bool selected;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!selected) return;
		
		foreach (IAbility a in abilities)
		{
			if (Input.GetMouseButtonDown(1)) a.on_rmouse();
			if (Input.GetKeyDown (KeyCode.Space)) a.on_space ();
		}
	}

}
