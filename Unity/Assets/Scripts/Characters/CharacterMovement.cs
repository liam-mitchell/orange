using UnityEngine;
using System.Collections;

public class CharacterMovement : IAbility {
	public float moveSpeed;
	public float turnSpeed;
	
	private float turn_duration_;
	private float turn_time_;
	private Quaternion turn_start_;
	private Quaternion turn_target_;

	private bool turn_update_;	
	private bool turning_;
	
	private Vector3 move_target_;
	private bool moving_;
	
	public override bool on_interrupt(int priority, IAbility source)
	{
		if (source == this) return true;
		if (priority > priority_) {
			moving_ = false;
			turning_ = false;
			turn_update_ = false;
			active_ = false;
			return true;
		} 
		else return false;
	}
	
	public override void on_rmouse()
	{
		Debug.Log("on_rmouse() (character_movement)");
		if (control.interrupt_all(priority_, this)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit))
			{
				update_target(hit.point);
			}
			
			active_ = true;
		}
	}
	
	protected override void update_animator()
	{
		animator_.SetBool("moving", moving_);
		Debug.Log("animator updated");
	}
	
	public void update_target(Vector3 target)
	{
		move_target_ = target;
		moving_ = false;
		turning_ = false;
		turn_update_ = true;
	}
	
	private void update_move()
	{		
		if (moving_) {
			move();
		}
	}
	
	private void update_turn()
	{
		if (turn_update_) {
			Vector3 direction = move_target_ - transform.position;
			turn_start_ = transform.rotation;
			turn_target_ = Quaternion.LookRotation(direction);
			turn_duration_ = Quaternion.Angle(turn_start_, turn_target_)
											/ turnSpeed;
			turn_time_ = 0;
			turn_update_ = false;
			turning_ = true;
		}
		
		if (turning_) {
			turn();
		}
	}
	
	private void move()
	{
		Vector3 direction = move_target_ - transform.position;
		if ((transform.position - move_target_).magnitude < .05) {
			moving_ = false;
			active_ = false;
		}
		else {
			transform.position += direction.normalized * moveSpeed * Time.deltaTime;
		}
	}
	
	private void turn()
	{
		transform.rotation = Quaternion.Slerp(turn_start_, turn_target_, turn_time_ / turn_duration_);
		turn_time_ += Time.deltaTime;
		if (turn_time_ > turn_duration_) {
			turning_ = false;
			moving_ = true;
		}
	}

	// Use this for initialization
	void Start () {
		animator_ = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate() {
		if (active_) {
			update_turn();
			update_move();
			update_animator();
		}
	}
}
