using UnityEngine;
using System.Collections;

public class FighterWhirlwind : IAbility {
	public float duration;
	public float tickDamage;
	public int ticks;
	public float range;

	private float tick_time_;
	private float current_tick_;

	private Animator animator_;

	public override void on_wkey()
	{
		if (!on_cooldown()
		    && !active_
		    && control.interrupt_all(priority_, this))
		{
			whirl();
		}
	}

	public override bool on_interrupt(int priority, IAbility source)
	{
		if (base.on_interrupt (priority, source)) {
			done_whirl();
			return true;
		}
		return false;
	}

	private void whirl()
	{
		active_ = true;
		current_tick_ = 0;
		tick_time_ = 0.0f;
		current_cooldown_ = cooldown;
	}

	private void update_whirl()
	{
		if (!active_) return;

		tick_time_ += Time.deltaTime;
		if (tick_time_ > duration / ticks) {
			tick_time_ = 0;
			tick();
		}
	}

	private void tick()
	{
		++current_tick_;
		GameObject [] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject e in enemies) {
			if (in_range(e)) e.SendMessage ("on_attack_damage", tickDamage);
		}
		if (current_tick_ >= ticks) done_whirl();
	}
	
	private bool in_range(GameObject o) {
		if ((o.transform.position - transform.position).magnitude > range) {
			return false;
		}
		return true;
	}

	private void update_animator()
	{
		animator_.SetBool ("whirling", active_);
	}

	private void done_whirl()
	{
		active_ = false;
		tick_time_ = 0;
		current_tick_ = 0;
	}

	override protected void done_turn() { /* empty */ }

	// Use this for initialization
	new void Start () {
		base.Start();
		animator_ = GetComponent<Animator>();
		tick_time_ = 0.0f;
		active_ = false;
		current_tick_ = 0;
	}
	
	// Update is called once per frame
	void Update () {
		update_whirl();
		update_animator();
		update_cooldown();
	}
}
