using UnityEngine;
using System.Collections;

public abstract class IEffect {
	protected static float STUN_PRIORITY = 100;

	protected IControl control_;
	protected float duration_;
	protected float current_time_;

	public IEffect(IControl control, float duration) {
		control_ = control;
		duration_ = duration;
		current_time_ = 0;
	}

	public abstract void start();

	public virtual bool tick() 
	{
		current_time_ += Time.deltaTime;
		if (current_time_ > duration_) {
			return true;
		}
		return false;
	}

	public abstract void end();
}
