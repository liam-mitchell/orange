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
		if (control.interrupt_all (priority_, this)) {
			active_ = true;
			tumble_start_ = transform.position;
			
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
		}
	}
	
	public override bool on_interrupt(int priority, IAbility source)
	{
		if (source == this || !active_) return true;
		if (priority >= priority_) {
			active_ = false;
			return true;
		}
		else return false;
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
		Debug.Log (animator_.GetBool("tumbling"));
	}

	// Use this for initialization
	void Start () {
		animator_ = GetComponent<Animator>();
		priority_ = 1;
		active_ = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (active_) {
			tumble ();
		}
		
		update_animator ();
		Debug.Log (animator_);
	}
}
