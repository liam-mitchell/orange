﻿using UnityEngine;
using System.Collections;

public class IAbility : MonoBehaviour {
	public CharacterControl control;
	protected Animator animator_;
	protected bool active_;
	protected int priority_;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected virtual void update_animator() { /* empty */ }
	
	public virtual void on_rmouse() { /* empty */ }
	
	/**
	 * interrupt
	 * interrupt this ability with the given priority
	 * (usually the priority of the ability attempting interrupt)
	 * 
	 * returns false if we have higher priority that
	 * the attempted interrupt
	 * 
	 * returns true without actually interrupting if
	 * we're the source
	 */
	public virtual bool on_interrupt(int priority, IAbility source)
	{
		return true; // dummy - override in child classes
	}
}
