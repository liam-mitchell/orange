using UnityEngine;
using System.Collections;

public class StunEffect : IEffect {
	private const int BASH_PRIORITY = 100;
		
	public StunEffect(IControl control, float duration)
		: base(control, duration) 
	{}
		
	override public void start() { /* empty */ }
		
	override public bool tick()
	{
		control_.interrupt_all(BASH_PRIORITY, null);
		return base.tick ();
	}
		
	override public void end()
	{
		control_.remove_effect(this);
	}
}
