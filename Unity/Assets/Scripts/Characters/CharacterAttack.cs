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
	public UserInterface userInterface; // to allow selecting of targets with the UI's functions
	public UnitStats stats;
	
	private float attack_duration_;
	private float attack_time_;
	
	private GameObject target_;
	private bool attacking_;
	private bool hit_this_attack_;
	
	private Animator animator_;
	
	public override void on_rmouse()
	{
		target_ = userInterface.mouseover_object();
		Debug.Log (target_);
		active_ = true;
	}
	
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
			return true;
		}
		return false;
	}
	
	private void attack()
	{
		attack_duration_ = stats.baseAttackTime * 100 / (100 + stats.agility);
		attack_time_ = 0;
		attacking_ = true;
		hit_this_attack_ = false;
	}
	
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
	
	private bool in_range()
	{
		if (target_ == null) return false;
		if ((target_.transform.position - transform.position).magnitude < 1.0f) return true;
		return false;
	}
	
	private void update_animator()
	{
		animator_.SetBool("attacking", attacking_);
		Debug.Log (animator_.GetBool ("attacking"));
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
		if (active_ && !attacking_) {
			if (in_range()
				&& control.interrupt_all(priority_, this))
			{
				attack();
			}
		}
		
		update_attack();			
		update_animator();
	}
}
