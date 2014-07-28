using UnityEngine;
using System.Collections.Generic;

/************************************************
 * IControl - interface for controlling an actor*
 * via abilities and interrupts					*
 * 												*
 * Closely related to the IAbility interface -	*
 * refer to that as well for a full description.*
 * 												*
 * Provides only the interrupt_all() function,	*
 * as well as maintaining a list of abilities,	*
 * which allows overriding controls to update	*
 * them as they see fit.						*
 ************************************************/

public abstract class IControl : MonoBehaviour {
	protected List<IAbility> abilities_;
	protected List<IModifier> modifiers_;
	protected List<IEffect> effects_;

	// Use this for initialization
	protected void Start () {
		IAbility[] abilities = GetComponents<IAbility>();
		abilities_ = new List<IAbility>();
		foreach (IAbility a in abilities) {
			abilities_.Add(a);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public virtual bool interrupt_all(int priority, IAbility source)
	{
		foreach (IAbility a in abilities_)
		{
			if (!a.on_interrupt(priority, source)) return false;
		}
		
		return true;
	}

	public void add_modifier(IModifier modifier)
	{
		modifiers_.Add (modifier);
	}

	public void remove_modifier(IModifier modifier)
	{
		modifiers_.RemoveAll (x => x == modifier);
	}

	public void add_effect(IEffect effect)
	{
		effects_.Add (effect);
	}

	public void remove_effect(IEffect effect)
	{
		effects_.RemoveAll (x => x == effect);
	}

	protected void add_nav_mesh_agent()
	{
		NavMeshHit hit;
		if (NavMesh.SamplePosition(transform.position,
		                           out hit,
		                           500,
		                           1))
		{
			transform.position = hit.position;
			gameObject.AddComponent<NavMeshAgent>();
		}
		else {
			Debug.Log ("Error adding nav mesh agent!");
		}
		
	}
}
