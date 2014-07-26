using UnityEngine;
using System.Collections;

public abstract class IModifier {
	protected IControl control_;
	
	protected float duration_;
	protected float current_time_;

	IModifier(IControl control, float duration) {
		control_ = control;
		duration_ = duration;
		current_time_ = 0;
	}

	protected void tick()
	{
		current_time_ += Time.deltaTime;
		if (current_time_ >= duration_) control_.remove_modifier(this);
	}

	abstract protected void modify(UnitStats stats);
}
