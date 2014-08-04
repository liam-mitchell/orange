using UnityEngine;
using System.Collections;

public class EnemyAttack : IAbility {
	public float damage;

	private EnemyControl enemy_control;

	private float attack_time_;
	private bool hit_this_attack_;
	private bool attacking_;

	private Animator animator_;

	public override bool on_interrupt(int priority, IAbility source)
	{
		if (base.on_interrupt(priority, source)) {
			done_attack();
			return true;
		}
		return false;
	}

	private void attack()
	{
		attack_time_ = 0.0f;
		attacking_ = true;
	}

	private void update_attack()
	{
		if (!active_) return;
		attack_time_ += Time.deltaTime;
		if (attack_time_ > 0.5f * stats.baseAttackTime
		    && !hit_this_attack_) 
		{
			hit();
		}
		if (attack_time_ > stats.baseAttackTime) {
			done_attack();
		}
	}

	private void update_animator()
	{
		animator_.SetBool ("attacking", attacking_);
	}

	private void hit()
	{
		enemy_control.target_.SendMessage("attack_damage", damage);
		hit_this_attack_ = true;
	}

	override protected void done_turn()
	{
		attack();
	}

	private void done_attack()
	{
		hit_this_attack_ = false;
		attack_time_ = 0.0f;
		active_ = false;
		attacking_ = false;
	}

	private bool in_range()
	{
		if (enemy_control.target_ == null) return false;
		if ((enemy_control.target_.transform.position
		     - transform.position).magnitude > 1.5f) 
		{
			return false;
		}
		return true;
	}
	// Use this for initialization
	new void Start () {
		base.Start();
		enemy_control = control as EnemyControl;
		animator_ = GetComponent<Animator>();
		priority_ = 0;
		hit_this_attack_ = false;
		attack_time_ = 0.0f;
		attacking_ = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (in_range()
		    && !active_
		    && control.interrupt_all(priority_, this))
		{
			turn(enemy_control.target_.transform.position);
			active_ = true;
		}

		update_turn();
		update_attack();
		update_animator();
	}
}
