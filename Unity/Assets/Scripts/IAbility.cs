using UnityEngine;
using System.Collections;

/****************************************************
 * IAbility - interface for all actions a player	*
 * or enemy can take in the game.					*
 * 													*
 * ***												*
 * on_<button>()									*
 * ***												*
 * This interface provides overridable callbacks	*
 * for all mouse and keyboard buttons. These		*
 * callbacks are guaranteed to be called whenever	*
 * the corresponding button is pressed, allowing	*
 * individual abilities to respond to them (or		*
 * not) as they see fit. These functions must be	*
 * overriden by any ability that wants to respond	*
 * to that particular message.						*
 * 													*
 * These functions are called in the closely		*
 * related IControl interface's update function - 	*
 * every frame if the appropriate button is down	*
 * that frame (or has been clicked that frame, in	*
 * the case of the mouse buttons).					*
 * 													*
 * ***												*
 * on_interrupt(int priority, IAbility source)		*
 * ***												*
 * This interface also provides the ability to		*
 * /interrupt/ another ability via					*
 * IControl::interrupt_all(). When an ability		*
 * wishes to start, it MUST first attempt to		*
 * interrupt other abilities, which will return		*
 * true if all other abilities were interrupted		*
 * successfully.									*
 * 													*
 * Then, the ability may proceed knowing it has		*
 * full control over the actor.						*
 * 													*
 * The source parameter is passed so that the		*
 * on_interrupt() function can check if it's		*
 * attempting to interrupt itself, and return		*
 * true if so without resetting itself.				*
 * 													*
 * 													*
 * NOTE: if an ability requires animation, it's		*
 * responsible for that itself. The standard way is	*
 * to acquire a reference to the animator in Start()*
 * and then simply update it with the relevant bool	*
 * in Update().										*
 ****************************************************/


public abstract class IAbility : MonoBehaviour {
	// reference to the control so we can interrupt
	// other abilities via interrupt_all()
	public IControl control;
	
	// cooldown of this ability
	public float cooldown;
	
	// are we active?
	protected bool active_;
	
	// priority to interrupt other abilities with
	// if we're interrupted by lower priority than
	// us, we don't care! refuse it
	protected int priority_;
	
	// how long until we can use this again?
	protected float current_cooldown_;
	
	/**
	 * on_*() - functions called when buttons are pressed
	 * or mouse buttons are clicked
	 * 
	 * MUST be overridden in subclasses - otherwise the
	 * empty virtual function will be called
	 * 
	 * Not abstract because then all abilities would be 
	 * forced to implement all of these as empty - so
	 * we provide a default BUT this means you have to
	 * remember to override them when you implement IAbility!
	 */
	public virtual void on_rmouse() { /* empty */ }
	public virtual void on_lmouse() { /* empty */ }
	public virtual void on_space() { /* empty */ }
	public virtual void on_qkey() { /* empty */ }
	
	/**
	 * on_interrupt() - interrupt this ability with the given priority
	 * (usually the priority of the ability attempting interrupt)
	 * 
	 * returns false if we have higher priority that
	 * the attempted interrupt
	 * 
	 * returns true without actually interrupting if
	 * we're the source
	 */
	public virtual bool on_interrupt(int priority, IAbility source)
	{
		if (source == this || !active_) return true;
		if (priority >= priority_) {
			active_ = false;
			return true;
		}
		else return false;
	}
}
