using UnityEngine;
using System.Collections;

public abstract class IModifier {
	protected IControl control_;
	
	protected float duration_;
	protected float current_time_;

	public IModifier(IControl control, float duration) {
		control_ = control;
		duration_ = duration;
		current_time_ = 0;
	}

	public bool tick()
	{
		current_time_ += Time.deltaTime;
		if (current_time_ >= duration_) return true;
		return false;
	}

	abstract public void modify(UnitStats stats);
}
