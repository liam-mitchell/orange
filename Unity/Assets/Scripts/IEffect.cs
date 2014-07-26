using UnityEngine;
using System.Collections;

public abstract class IEffect : MonoBehaviour {
	private IControl control_;
	private float duration_;
	private float current_time_;

	public IEffect(IControl control, float duration) {
		control_ = control;
		duration_ = duration;
	}

	public abstract void start();
	public abstract void tick();
	public abstract void end();
}
