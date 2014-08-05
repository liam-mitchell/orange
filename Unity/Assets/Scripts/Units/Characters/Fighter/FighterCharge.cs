using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FighterCharge : IAbility {
	public float chargeSpeed;
	public float chargeDistance;
	public float stunRadius;
	public float stunDuration;

	private Vector3 charge_start_;
	private Vector3 charge_end_;
	private float charge_time_;
	private float charge_duration_;

	private Animator animator_;

	public override void on_space()
	{
		if (!on_cooldown ()
		    && !active_
		    && control.interrupt_all(priority_, this))
		{
			charge();
		}
	}

	private void charge()
	{
		if (!cast()) return;
		active_ = true;
		charge_start_ = transform.position;

		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, chargeDistance)) {
			charge_end_ = hit.point - transform.forward * 0.5f;
		}
		else {
			charge_end_ = transform.position + transform.forward * chargeDistance;
		}

		charge_duration_ = (charge_end_ - charge_start_).magnitude / chargeSpeed;
		charge_time_ = 0;

		current_cooldown_ = cooldown;
	}

	private void update_charge()
	{
		if (!active_) return;
		charge_time_ += Time.deltaTime;
		if (charge_time_ >= charge_duration_) end_charge();

		transform.position = Vector3.Slerp(charge_start_,
		                                   charge_end_,
		                                   charge_time_ / charge_duration_);
	}

	private void end_charge()
	{
		GameObject [] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject e in enemies) {
			if (can_stun(e)) {
				IControl enemy_control = e.GetComponent<IControl>();
				enemy_control.add_effect(new StunEffect(enemy_control,
				                                        stunDuration));
			}
		}
		active_ = false;
	}

	private bool can_stun(GameObject o)
	{
		if ((o.transform.position - transform.position).magnitude > stunRadius) return false;
		return true;
	}

	private void update_animator()
	{
		animator_.SetBool("charging", active_);
	}

	protected override void done_turn() { /* empty */ }

	// Use this for initialization
	new void Start () {
		base.Start ();
		animator_ = GetComponent<Animator>();
		priority_ = 2;
		active_ = false;
	}
	
	// Update is called once per frame
	void Update () {
		update_charge();
		update_animator();
		update_cooldown();
	}
}
