﻿using UnityEngine;
using System.Collections;

public class StunEffect : IEffect {
	private const int BASH_PRIORITY = 100;
	private Animator animator_;
		
	public StunEffect(IControl control, float duration)
		: base(control, duration) 
	{
		animator_ = control.GetComponent<Animator>();
	}
		
	override public bool tick()
	{
		control_.interrupt_all(BASH_PRIORITY, null);
		animator_.SetBool ("moving", false);
		animator_.SetBool ("attacking", false);
		return base.tick ();
	}
		
	override public void end()
	{

	}
}
