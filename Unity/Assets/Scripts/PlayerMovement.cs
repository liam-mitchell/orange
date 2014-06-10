using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
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
	
	private Animator animator_;
	
	private void update_input()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Input.GetMouseButtonDown(1)
			&& Physics.Raycast (ray, out hit))
		{
			move_target_ = hit.point;
			turn_update_ = true;
			moving_ = false;
			turning_ = false;
		}
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
			Debug.Log (turn_start_);
			Debug.Log (turn_target_);
			Debug.Log (turn_duration_);
		}
		
		if (turning_) {
			turn();
		}
	}
	
	private void update_animator()
	{
		animator_.SetBool ("moving", moving_);
	}
	
	private void move()
	{
		Vector3 direction = move_target_ - transform.position;
		if ((transform.position - move_target_).magnitude < .05) {
			moving_ = false;
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
		update_input ();
	}
	
	void FixedUpdate() {
		update_turn();
		update_move();
		update_animator ();
	}
}
