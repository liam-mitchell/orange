using UnityEngine;
using System.Collections.Generic;

public class CharacterControl : IControl
{
	// Use this for initialization
	void Start () {
		abilities_ = new List<IAbility> {
			GetComponent<CharacterMovement>()
		};
	}
	
	// Update is called once per frame
	void Update () {
		foreach (IAbility a in abilities_)
		{
			Debug.Log("There's one!");
			if (Input.GetMouseButtonDown(1)) a.on_rmouse();
		}
	}

}
