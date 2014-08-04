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
 *													*
 * ***												*
 * target()	/ done_targeting()						*
 * ***												*
 * 													*
 * Helper functions for acquiring a target via the	*
 * user interface (blocking it from selecting on	*
 * the next left mouse click).						*
 *													*
 * ***												*
 * turn(Vector3 target) / update_turn()				*
 *													*
 * Helper functions for abilities which require		*
 * facing the target. turn() will initiate a turn to*
 * face target, while update_turn() will be called	*
 * every frame. On completion, update_turn() will	*
 * send a 'done_turn()' message, to be accepted in	*
 * the ability which is initiating the turn.		*
 ****************************************************/ 


public abstract class IAbility : MonoBehaviour {
	// reference to the control so we can interrupt
	// other abilities via interrupt_all()
	protected IControl control;
	protected UnitStats stats;

	protected UserInterface userInterface;
	
	// cooldown of this ability
	public float cooldown;
	
	// are we active?
	protected bool active_;
	
	// for abilities requiring a target phase
	protected bool targeting_;
	protected GameObject target_;
	
	// for abilities that require facing the target
	protected float turn_duration_;
	protected float turn_time_;
	protected Quaternion turn_start_;
	protected Quaternion turn_target_;
	protected bool turning_;
	
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
	public virtual void on_rmouse() { done_targeting(); } // by default, cancel targeting on rmouse TODO: this might be better as on_interrupt()?
	public virtual void on_lmouse() { /* empty */ }
	public virtual void on_space() { /* empty */ }
	public virtual void on_qkey() { /* empty */ }
	public virtual void on_wkey() { /* empty */ }
	public virtual void on_ekey() { /* empty */ }
	public virtual void on_rkey() { /* empty */ }
	
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

	/**
	 * target() - acquires a target through the UI
	 * requires done_targeting() to be called when we're
	 * finished stealing the left mouse
	 */
	protected virtual void target()
	{
		if (userInterface == null) userInterface = GetComponentInParent<UserInterface>();
		userInterface.targeting = true;
		targeting_ = true;
	}
	
	protected virtual void done_targeting()
	{
		if (userInterface == null) userInterface = GetComponentInParent<UserInterface>();
		userInterface.targeting = false;
		targeting_ = false;
	}
	
	/**
	 * turn() - initiates a turning motion to face towards
	 * target.
	 * 
	 * REQUIRES: 
	 *  -that update_turn() be called once per frame
	 *     by the ability that initiated the turn
	 *  -that the ability that initiated the turn
	 * 	   implement the done_turn() function, to be called
	 *     when update_turn() finds we're done
	 */
	protected virtual void turn(Vector3 target)
	{
		Vector3 direction = target - transform.position;
		direction.y = 0;
		turn_start_ = transform.rotation;
		turn_target_ = Quaternion.LookRotation(direction);
		turn_duration_ = Quaternion.Angle(turn_start_, turn_target_)
										/ stats.turnspeed;
		turn_time_ = 0;
		turning_ = true;
	}
	
	/**
	 * update_turn() - updates turning motion in progress
	 * 
	 * call every frame in any ability that requires
	 * facing a target
	 */
	protected virtual void update_turn()
	{
		if (!turning_) return;
		
		if (turn_duration_ <= 0.0f) {
			turning_ = false;
			done_turn();
			return;
		}
		
		transform.rotation = Quaternion.Slerp(turn_start_, turn_target_, turn_time_ / turn_duration_);
		turn_time_ += Time.deltaTime;
		if (turn_time_ > turn_duration_) {
			turning_ = false;
			done_turn ();
		}
	}
	
	/**
	 * done_turn() - empty implementation to be overriden
	 * in child classes
	 */
	protected abstract void done_turn();
	
	protected virtual void update_cooldown()
	{
		if (current_cooldown_ >= 0) current_cooldown_ -= Time.deltaTime;
	}

	protected bool on_cooldown()
	{
		return current_cooldown_ >= 0;
	}
	
	protected void Start()
	{
		current_cooldown_ = -1;
		control = GetComponent<IControl>();
		userInterface = GetComponentInParent<UserInterface>();
		stats = GetComponent<UnitStats>();
	}
}
