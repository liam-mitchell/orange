using UnityEngine;
using System.Collections;

public class ParryEffect : IEffect {
	private attack_damage_handler_t old_attack_handler_;
	private int blocks_;
	private Animator animator_;

	private const float parry_duration_ = 0.25f;
	private float time_;
	private bool parrying_;

	public ParryEffect(IControl control, float duration, int blocks)
		: base(control, duration)
	{
		blocks_ = blocks;
		time_ = 0;

		animator_ = control_.GetComponent<Animator>();

		old_attack_handler_ = control_.attack_damage_handler;
		control_.attack_damage_handler = on_attack_damage;
	}

	public void on_attack_damage(float damage)
	{
		--blocks_;
		old_attack_handler_(0.0f);
		parrying_ = true;
	}

	override public void end()
	{
		control_.attack_damage_handler = old_attack_handler_;
		animator_.SetBool ("parrying", false);
	}

	private void update_animator()
	{
		animator_.SetBool ("parrying", parrying_);
	}

	private void update_parry()
	{
		if (!parrying_) return;
		time_ += Time.deltaTime;
		if (time_ > parry_duration_) {
			parrying_ = false;
			time_ = 0;
		}
	}

	public override bool tick()
	{
		update_parry();
		update_animator();
		if (base.tick ()) return true;
		if (blocks_ <= 0) return true;
		return false;
	}
}
