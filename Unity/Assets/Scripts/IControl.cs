using UnityEngine;
using System.Collections.Generic;

/************************************************
 * IControl - interface for controlling an actor*
 * via abilities and interrupts					*
 * 												*
 * Closely related to the IAbilty interface -	*
 * refer to that as well for a full description.*
 * 												*
 * Provides only the interrupt_all() function,	*
 * as well as maintaining a list of abilities,	*
 * which allows overriding controls to update	*
 * them as they see fit.						*
 ************************************************/

public abstract class IControl : MonoBehaviour {
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
