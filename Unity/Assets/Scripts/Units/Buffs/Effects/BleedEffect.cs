using UnityEngine;
using System.Collections;

public class BleedEffect : IEffect {
	private float tick_damage_;
	private float tick_time_;
	private int ticks_;
	private int current_tick_;
	
	public BleedEffect(IControl control, float duration, float damage, int ticks) : base(control, duration)
	{
		tick_damage_ = damage;
		tick_time_ = 0.0f;
		ticks_ = ticks;
		current_tick_ = 0;
	}
	
	public override bool tick()
	{
		tick_time_ += Time.deltaTime;

		if (tick_time_ > duration_ / ticks_) {
			++current_tick_;
			tick_time_ = 0;
			control_.SendMessage("on_attack_damage", tick_damage_);
			if (current_tick_ > ticks_) return true;
		}
		
		return false;
	}
	
	public override void end()
	{
		
	}
}