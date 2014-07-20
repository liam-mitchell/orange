using UnityEngine;
using System.Collections;

public class RogueTumble : IAbility {
	public float tumbleSpeed;
	public float tumbleDistance;
	
	private Vector3 tumble_start_;
	private Vector3 tumble_end_;
	private float tumble_duration_;
	private float tumble_time_;
	
	private Animator animator_;
	
	public override void on_space()
	{
		if (!(current_cooldown_ > 0)
			&& !active_
			&& control.interrupt_all (priority_, this)) 
		{
			active_ = true;
			tumble_start_ = transform.position + Vector3.up * .5f;
			
			RaycastHit hit;
			if (Physics.Raycast (transform.position,
								 transform.forward,
								 out hit,
								 tumbleDistance))
			{
				tumble_end_ = hit.point - transform.forward * 0.5f;
			}
			else {
				tumble_end_ = transform.position + transform.forward * tumbleDistance;
			}
			
			tumble_duration_ = (tumble_end_ - tumble_start_).magnitude / tumbleSpeed;
			tumble_time_ = 0;
			
			current_cooldown_ = cooldown;
		}
	}
	
	private void tumble()
	{
		transform.position = Vector3.Slerp(tumble_start_,
										   tumble_end_,
										   tumble_time_ / tumble_duration_);
		if (tumble_time_ > tumble_duration_) {
			active_ = false;
		}
		else {
			tumble_time_ += Time.deltaTime;
		}
	}
	
	private void update_animator()
	{
		animator_.SetBool("tumbling", active_);
	}

	// Use this for initialization
	new void Start () {
		animator_ = GetComponent<Animator>();
		priority_ = 2;
		active_ = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (active_) {
			tumble ();
		}
		
		if (current_cooldown_ > 0) {
			current_cooldown_ -= Time.deltaTime;
		}
		
		update_animator ();
	}
}
