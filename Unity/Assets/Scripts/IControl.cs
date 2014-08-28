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

public delegate void attack_damage_handler_t(float damage);

public abstract class IControl : MonoBehaviour {
	protected List<IAbility> abilities_;
	protected List<IModifier> modifiers_;
	protected List<IEffect> effects_;

	private UnitStats base_stats_;

	public attack_damage_handler_t attack_damage_handler;

	// Use this for initialization
	protected void Start () {
		IAbility[] abilities = GetComponents<IAbility>();
		abilities_ = new List<IAbility>();
		foreach (IAbility a in abilities) {
			abilities_.Add(a);
		}

		modifiers_ = new List<IModifier>();
		effects_ = new List<IEffect>();

		base_stats_ = GetComponent<UnitStats>();
		attack_damage_handler = new attack_damage_handler_t(base_stats_.on_attack_damage);
	}
	
	// Update is called once per frame
	protected void Update () {
		update_effects();
		update_modifiers();
	}

	private void update_modifiers()
	{
		List <IModifier> dead_modifiers = new List<IModifier>();

		UnitStats stats = GetComponent<UnitStats>();

		stats.reset_modifiers (base_stats_);
		base_stats_ = stats.clone ();

		foreach (IModifier m in modifiers_)
		{
			m.modify(stats);
			if (m.tick()) dead_modifiers.Add (m);
		}

		foreach (IModifier m in dead_modifiers)
		{
			remove_modifier(m);
		}
	}

	private void update_effects()
	{
		List<IEffect> dead_effects = new List<IEffect>();
		foreach (IEffect e in effects_) {
			if (e.tick ()) {
				dead_effects.Add (e);
			}
		}
		
		foreach (IEffect e in dead_effects) {
			e.end();
			remove_effect(e);
		}

	}

	public void attack_damage(float damage)
	{
		attack_damage_handler(damage);
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
		Debug.Log (effect);
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
