using UnityEngine;
using System.Collections;

public class CharacterAttack : IAbility {
	/* Attack speed is measured in percentage of original -
	 * so everyone starts at 1, then 2 is 2x as fast,
	 * 3 is 3x as fast, etc.
	 * 
	 * Using the formula: t = b / s 
	 * where t = final attack time
	 * 		 b = base attack time
	 * 		 s = attack speed
	 */	
	private float attack_duration_;
	private float attack_time_;
	
	private bool attacking_;
	private bool hit_this_attack_;
	
	private Animator animator_;
	
	/**
	 * on_rmouse() - called when right mouse is clicked
	 * overrides IAbility::on_rmouse() - see for details
	 */
	public override void on_rmouse()
	{
		target_ = userInterface.mouseover_object();
		active_ = true;
	}
	
	/**
	 * on_interrupt() - called when someone tries to
	 * interrupt us
	 * overrides IAbility::on_interrupt() - see for details
	 */
	public override bool on_interrupt(int priority, IAbility source)
	{
		if (source == this || !active_) return true;
		if (priority >= priority_) {
			active_ = false;
			attacking_ = false;
			attack_time_ = 0;
			attack_duration_ = 0;
			target_ = null;
			hit_this_attack_ = false;
			update_animator();
			return true;
		}
		return false;
	}
	
	/**
	 * attack() - begins attack
	 * pre: in_range()
	 */
	private void attack()
	{
		attack_duration_ = stats.baseAttackTime * 100 / (100 + stats.agility);
		attack_time_ = 0;
		attacking_ = true;
		hit_this_attack_ = false;
		update_animator();
	}
	
	protected override void done_turn()
	{
		attack ();
	}	
	/**
	 * update_attack() - called every frame
	 * if we're attacking, updates the time and
	 * does damage if we're past the point of
	 * no return
	 */
	private void update_attack()
	{
		if (!attacking_) return;
		
		attack_time_ += Time.deltaTime;
		
		if (attack_time_ > 0.5f * attack_duration_
			&& !hit_this_attack_)
		{
			target_.SendMessage("on_attack_damage", stats.attack_damage());
			hit_this_attack_ = true;
		}
		else if (attack_time_ > attack_duration_) {
			attack();
		}
	}
	
	/**
	 * in_range() - are we in range to hit the target?
	 */
	private bool in_range()
	{
		if (target_ == null) return false;
		if ((target_.transform.position - transform.position).magnitude < 1.0f) return true;
		return false;
	}
	
	private void update_animator()
	{
		animator_.SetBool("attacking", attacking_);
	}
	
	// Use this for initialization
	void Start () {
		animator_ = GetComponent<Animator>();
		attack_time_ = 0;
		attack_duration_ = 0;
		active_ = false;
		attacking_ = false;
		priority_ = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (active_ 
			&& !attacking_
			&& !turning_
			&& in_range ()
			&& control.interrupt_all(priority_, this)) {
				turn (target_.transform.position);
		}
		
		update_turn ();
		update_attack();
	}
}
