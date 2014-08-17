using UnityEngine;
using System.Collections;

public class RogueShank : IAbility {
	public float damage;
	public float duration;
	
	private Animator animator_;
	
	private bool shanking_;
	
	private float shank_time_;
	
	/**
	 * on_qkey() - overrides IAbility::on_qkey()
	 * called when q is pressed on the keyboard
	 * see IAbility::on_qkey() for details
	 */
	public override void on_qkey()
	{
		target();
	}
	
	/**
	 * on_lmouse() - overrides IAbility::on_lmouse()
	 * called when the left mouse button is clicked
	 * see IAbility::on_lmouse() for details
	 */
	public override void on_lmouse()
	{
		if (targeting_)
		{
			target_ = userInterface.mouseover_object();	
			if (can_shank () && control.interrupt_all(priority_, this)) {
				turn (target_.transform.position);
			}
			done_targeting();
		}
	}
	
	/**
	 * on_interrupt() - overrides IAbility::on_interrupt() to properly
	 * shut down this ability if interruptible
	 * see IAbility::on_interrupt() for details
	 */
	public override bool on_interrupt(int priority, IAbility source)
	{
		if (source == this || !active_) return true;
		if (priority >= priority_) {
			active_ = false;
			shanking_ = false;
			targeting_ = false;
			shank_time_ = 0;
			target_ = null;
			return true;
		}
		
		return false;
	}
	
	protected override void done_turn()
	{
		shank();
	}
	
	/**
	 * shank() - called when in range of a target to begin the
	 * shank ability
	 * 
	 * pre: can_shank()
	 * 
	 * attempts to interrupt all other abilities as per
	 * IAbility spec
	 */
	private void shank()
	{		
		if (!cast()) return;
		shanking_ = true;
		targeting_ = false;
		shank_time_ = 0;
		current_cooldown_ = cooldown;
		update_animator();
	}
	
	/**
	 * called every frame in update()
	 * updates cooldown, and updates the shank by sending damage
	 * message if we're appropriately far into the shank
	 * as well as stopping the shank if we're done
	 */
	private void update_shank()
	{
		if (!shanking_) return;
		
		shank_time_ += Time.deltaTime;
		
		if (shank_time_ >= duration) {
			target_.SendMessage ("on_attack_damage", damage);
			shanking_ = false;
			update_animator();
		}
	}
	
	/**
	 * can_shank() - can we shank()?
	 * we must have:
	 *  a target
	 *  no cooldown
	 *  be in range
	 *  be behind target
	 */
	private bool can_shank()
	{
		if (target_ == null) return false;
		if (current_cooldown_ > 0) return false;
		
		Vector3 attack_direction = target_.transform.position - transform.position;
		
		if (attack_direction.magnitude > 1.0f) return false;
		
		return true;
	}
	
	/**
	 * update_animator() - for any ability that requires animation
	 */
	private void update_animator()
	{
		animator_.SetBool("shanking", shanking_);
	}
	// Use this for initialization
	new void Start () {
		base.Start();
		priority_ = 2;
		targeting_ = false;
		shanking_ = false;
		shank_time_ = 0;
		current_cooldown_ = 0;
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		update_turn ();
		update_shank ();
		update_cooldown ();
	}
}
