using UnityEngine;
using System.Collections.Generic;

public class IControl : MonoBehaviour {
	public List<IAbility> abilities;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public virtual bool interrupt_all(int priority, IAbility source)
	{
		foreach (IAbility a in abilities)
		{
			if (!a.on_interrupt(priority, source)) return false;
		}
		
		return true;
	}
}
